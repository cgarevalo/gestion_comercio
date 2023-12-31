using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;
using System.Net;
using System.Threading;

namespace Aplicacion
{
    public partial class Principal : Form
    {
        TiendaNegocio negocio = new TiendaNegocio();
        List<Producto> listaProductos;
        List<Producto> carrito;
        int cantidadBandera = 0;
        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            Refrescar();
            carrito = negocio.RecargarCarrito();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaProducto alta = new frmAltaProducto();
            alta.ShowDialog();
            Refrescar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                Producto prodSeleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;

                frmModificarProducto modificar = new frmModificarProducto(prodSeleccionado);
                modificar.ShowDialog();
                Refrescar();
            }
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                Producto prodSeleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;

                frmMostrarDetalles detalles = new frmMostrarDetalles(prodSeleccionado);
                detalles.ShowDialog();
                Refrescar();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Eliminando producto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    Producto prodSeleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                    negocio.Eliminar(prodSeleccionado.ID);
                    Refrescar();
                }
            }
        }

        private void btnCarrito_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                Producto seleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                AgregarCarrito(seleccionado);
                Refrescar();
            }
        }

        private void btnVerCarrito_Click(object sender, EventArgs e)
        {
            frmCarrito chango = new frmCarrito(carrito);
            chango.ShowDialog();
            Refrescar();
        }

        private void Refrescar()
        {
            listaProductos = negocio.ListarProductos();
            dgvProductos.DataSource = listaProductos;
            CargarImagen(listaProductos[0].ImagenUrl);
            OcultarColumnas();
        }

        private void OcultarColumnas()
        {
            // Oculta tres columnas de la tabla
            dgvProductos.Columns["ID"].Visible = false;
            dgvProductos.Columns["ImagenUrl"].Visible = false;
            dgvProductos.Columns["FechaRegistro"].Visible = false;
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

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            // Muestra la imagen del artículo seleccionado en el PictureBox.
            if (dgvProductos.CurrentRow != null)
            {
                Producto productoSeleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                CargarImagen(productoSeleccionado.ImagenUrl);
            }
        }

        private void AgregarCarrito(Producto producto)
        {
            // Verificar si el producto ya está en el carrito
            if (carrito.Any(p => p.ID == producto.ID))
            {
                MessageBox.Show("El producto ya se encuentra en el carrito.");
                return;
            }

            // Utiliza un método separado para obtener la cantidad
            if (ObtenerCantidad(out int cantidad))
            {
                producto.Cantidad = cantidad;

                // Verifica si hay stock
                if (producto.Cantidad >= cantidad)
                {
                    carrito.Add(producto);
                    negocio.CargarCarrito(producto.ID, cantidad);
                    MessageBox.Show("Agregado al carrito.");
                }
                else
                {
                    MessageBox.Show("Sin stock.");
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private bool ObtenerCantidad(out int cantidad)
        {
            // Muestra un cuadro de diálogo simple para ingresar la cantidad (Para utiliza VisualBasic hay que agregar la referencia Microsoft.VisualBasic)
            string cantidadStr = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la cantidad:", "Cantidad", "1");

            // Verifica si el cuadro de diálogo se cerró sin ingresar un valor
            if (string.IsNullOrWhiteSpace(cantidadStr))
            {
                cantidad = 0;
                return false;
            }

            if (int.TryParse(cantidadStr, out cantidad))
            {
                if (cantidad > 0)
                {
                    cantidadBandera = cantidad;
                    return true;
                }
                else
                {
                    MessageBox.Show("Ingrese números válidos, números mayores a 0.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Ingrese un número.");
                return false;
            }
        }
    }
}
