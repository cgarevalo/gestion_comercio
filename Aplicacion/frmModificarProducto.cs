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
    public partial class frmModificarProducto : Form
    {
        Producto producto;
        TiendaNegocio negocio;

        // Constructor utilizado para modificar un producto existente, recibe un producto como parámetro
        public frmModificarProducto(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
        }

        private void frmModificarProducto_Load(object sender, EventArgs e)
        {
            string nombre = producto.Nombre;
            string descripcion = producto.Descripcion;
            string imagen = producto.ImagenUrl;
            int cantidad = producto.Cantidad;
            decimal precio = producto.Precio;

            txtNombre.Text = nombre;
            txtDescripcion.Text = descripcion;
            txtUrlImagen.Text = imagen;
            nudCantidad.Value = cantidad;
            txtPrecio.Text = precio.ToString();

            CargarImagen(imagen);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            negocio = new TiendaNegocio();

            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            string imagen = txtUrlImagen.Text;
            int cantidad;
            decimal precio;

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(descripcion) && !string.IsNullOrWhiteSpace(imagen))
            {
                producto.Nombre = nombre;
                producto.Descripcion = descripcion;
                producto.ImagenUrl = imagen;
            }
            else
            {
                MessageBox.Show("Debe llenar todos los campos.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtPrecio.Text) && !string.IsNullOrWhiteSpace(nudCantidad.ToString()))
            {
                if (decimal.TryParse(txtPrecio.Text, out precio) && int.TryParse(nudCantidad.Text, out cantidad))
                {
                    if (precio >= 0 && cantidad >= 0)
                    {
                        producto.Precio = precio;
                        producto.Cantidad = cantidad;
                    }
                    else
                    {
                        MessageBox.Show("Ingrese números válidos.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Cantidad y Precio deben tener números.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Debe llenar todos los campos.");
                return;
            }

            negocio.Modificar(producto);
            MessageBox.Show("Producto modificado.");
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pboImagen.Load(imagen);
            }
            catch
            {
                pboImagen.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }

        private void txtUrlImagen_TextChanged(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }
    }
}
