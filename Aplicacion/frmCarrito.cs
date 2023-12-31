using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion
{
    public partial class frmCarrito : Form
    {
        List<Producto> carrito;
        TiendaNegocio negocio = new TiendaNegocio();

        public frmCarrito(List<Producto> carrito)
        {
            InitializeComponent();
            this.carrito = carrito;
        }

        private void frmCarrito_Load(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvcarrito.SelectedRows.Count > 0)
            {
                Producto selecionado = (Producto)dgvcarrito.CurrentRow.DataBoundItem;
                EliminarDelCarrito(selecionado.ID);
                MessageBox.Show("Producto eliminado del carrito.");
                this.Close();
            }           
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comprado.");
        }

        private void Refrescar()
        {
            dgvcarrito.DataSource = carrito;

            // Verificar si hay elementos en carrito antes de intentar acceder al primer elemento
            if (carrito.Any())
            {
                CargarImagen(carrito[0].ImagenUrl);
            }
            else
            {
                return;
            }
            
            OcultarColumnas();
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch
            {
                pbxImagen.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }

        private void OcultarColumnas()
        {
            // Oculta tres columnas de la tabla
            dgvcarrito.Columns["ID"].Visible = false;
            dgvcarrito.Columns["ImagenUrl"].Visible = false;
            dgvcarrito.Columns["FechaRegistro"].Visible = false;
        }

        private void EliminarDelCarrito(int id)
        {
            Producto productoEliminar = carrito.Find(p => p.ID == id);
            carrito.Remove(productoEliminar);
            negocio.EliminarDelCarrito(id);
        }
    }
}
