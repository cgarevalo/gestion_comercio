using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Dominio;

namespace Negocio
{
    public class TiendaNegocio
    {
        AccesoDatos datos = new AccesoDatos();
        List<Producto> productos;
        int id;

        public List<Producto> ListarProductos()
        {
            productos = new List<Producto>();

            try
            {
                datos.SetearConsulta("SELECT ID, Nombre, Descripcion, Precio, Cantidad, ImagenUrl, FechaRegistro FROM Productos WHERE Activo = 1");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Producto producto = new Producto();

                    producto.ID = (int)datos.Lector["ID"];
                    id = producto.ID;
                    producto.Nombre = (string)datos.Lector["Nombre"];
                    producto.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        producto.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    }
                    
                    producto.Precio = (decimal)datos.Lector["Precio"];
                    producto.Cantidad = (int)datos.Lector["Cantidad"];
                    producto.FechaRegistro = (DateTime)datos.Lector["FechaRegistro"];

                    productos.Add(producto);
                }

                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Agregar(Producto producto)
        {
            try
            {
                datos.SetearConsulta("INSERT INTO Productos (Nombre, Descripcion, ImagenUrl, Precio, Cantidad, FechaRegistro) VALUES (@nombre, @descripcion, @imagenUrl, @precio, @cantidad, @fechaRegistro)");

                datos.SetearParametro("@nombre", producto.Nombre);
                datos.SetearParametro("@descripcion", producto.Descripcion);
                datos.SetearParametro("@imagenUrl", producto.ImagenUrl);
                datos.SetearParametro("@precio", producto.Precio);
                datos.SetearParametro("@cantidad", producto.Cantidad);
                datos.SetearParametro("@fechaRegistro", producto.FechaRegistro);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Producto producto)
        {
            try
            {
                datos.SetearConsulta("UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, ImagenUrl = @imagenUrl, Precio = @precio, Cantidad = @cantidad WHERE ID = @id");

                datos.SetearParametro("@nombre", producto.Nombre);
                datos.SetearParametro("@descripcion", producto.Descripcion);
                datos.SetearParametro("@imagenUrl", producto.ImagenUrl);
                datos.SetearParametro("@precio", producto.Precio);
                datos.SetearParametro("@cantidad", producto.Cantidad);
                datos.SetearParametro("@id", producto.ID);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        
        public void Eliminar(int id)
        {
            try
            {
                datos.SetearConsulta("UPDATE Productos SET Activo = 0 WHERE ID = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ValidarCredenciales(string usuario, string contraseña)
        {
            try
            {
                datos.SetearConsulta("SELECT NombreUsuario, Contraseña FROM Administradores WHERE NombreUsuario = @usuario AND Contraseña = @contraseña");

                datos.SetearParametro("@usuario", usuario);
                datos.SetearParametro("@contraseña", contraseña);
                datos.EjecutarConsulta();

                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar credenciales: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void RegistrarAdmin(string usuario, string contraseña)
        {
            try
            {
                datos.SetearConsulta("INSERT INTO Administradores (NombreUsuario, Contraseña) VALUES (@nombreUsuario, @contraseña)");

                datos.SetearParametro("@nombreUsuario", usuario);
                datos.SetearParametro("@contraseña", contraseña);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar admin: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Guarda el producto seleccionado en la base de datos
        public void CargarCarrito(int idproducto, int cantidad)
        {
            try
            {
                // Agrega el producto al carrito en la tabla Carrito
                datos.SetearConsulta("INSERT INTO Carrito (IdProducto, Cantidad) VALUES (@idProducto, @cantidad)");
                datos.SetearParametro("@idProducto", idproducto);
                datos.SetearParametro("@cantidad", cantidad);
                datos.EjecutarAccion();

                // Cierra la conexión después de la primera consulta
                datos.CerrarConexion();

                // Disminuye la cantidad del producto en la tabla Productos
                datos.SetearConsulta("UPDATE Productos SET Cantidad = Cantidad - @cantidad2 WHERE ID = @idProducto");
                datos.SetearParametro("@idProducto", idproducto);
                datos.SetearParametro("@cantidad2", cantidad);
                datos.EjecutarConsulta();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar al carrito: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Carga los productos de la tabla Carrito desde la base de datos
        public List<Producto> RecargarCarrito()
        {
            try
            {
                int idProducto;
                int cantidad;
                List<Producto> carrito = new List<Producto>();

                datos.SetearConsulta("SELECT IdProducto, Cantidad FROM Carrito");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    // GetIn32() obtiene el valor de la columna especificada entre paréntesis
                    idProducto = datos.Lector.GetInt32(0); // Toma el valor de IdProducto
                    cantidad = datos.Lector.GetInt32(1); // Toma el valor de Cantidad

                    Producto productoCarrito = productos.Find(p => p.ID == idProducto);
                    if (productoCarrito != null)
                    {
                        productoCarrito.Cantidad = cantidad;
                        carrito.Add(productoCarrito);
                    }
                }

                return carrito;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar los productos al carro: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarDelCarrito(int id)
        {
            try
            {
                datos.SetearConsulta("DELETE FROM Carrito WHERE IdProducto = @idCarrito");
                datos.SetearParametro("@idCarrito", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar del carrito: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
