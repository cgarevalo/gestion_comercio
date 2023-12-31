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
    public partial class frmAltaProducto : Form
    {
        TiendaNegocio negocio = new TiendaNegocio();

        public frmAltaProducto()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Producto producto = new Producto();
            

            try
            {
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                string imagen = txtImagenUrl.Text;
                int cantidad;
                decimal precio;

                if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(descripcion) && !string.IsNullOrWhiteSpace(imagen))
                {
                    producto.Nombre = nombre;
                    producto.Descripcion = descripcion;
                    producto.ImagenUrl = imagen;
                    producto.FechaRegistro = DateTime.Now;
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

                negocio.Agregar(producto);
                MessageBox.Show("Producto agregado.");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message);
            }
        }

        private void txtImagenUrl_TextChanged(object sender, EventArgs e)
        {
            CargarImagen(txtImagenUrl.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagenAlta.Load(imagen);
            }
            catch
            {
                pbxImagenAlta.Load("https://image.shutterstock.com/image-vector/ui-image-placeholder-wireframes-apps-260nw-1037719204.jpg");
            }
        }
    }
}
