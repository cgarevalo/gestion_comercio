using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Aplicacion
{
    public partial class frmMostrarDetalles : Form
    {
        Producto producto;
        List<Producto> listaProducto;

        public frmMostrarDetalles(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
        }

        private void frmMostrarDetalles_Load(object sender, EventArgs e)
        {
            string nombre = producto.Nombre;
            string descripcion = producto.Descripcion;
            string imagen = producto.ImagenUrl;
            decimal precio = producto.Precio;
            int id = producto.ID;
            int cantidad = producto.Cantidad;
            DateTime fecha = producto.FechaRegistro;

            dgvDetalles.Rows.Add("ID", id);
            dgvDetalles.Rows.Add("Nombre", nombre);
            dgvDetalles.Rows.Add("Descripción", descripcion);
            dgvDetalles.Rows.Add("Precio", precio);
            dgvDetalles.Rows.Add("Cantidad", cantidad);
            dgvDetalles.Rows.Add("Url de la imágen", imagen);
            dgvDetalles.Rows.Add("Fecha de registro", fecha);

            CargarImagen(producto.ImagenUrl);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
