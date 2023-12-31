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
    public partial class frmLogin : Form
    {
        TiendaNegocio negocio;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            negocio = new TiendaNegocio();
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            if (negocio.ValidarCredenciales(usuario, contraseña))
            {
                MessageBox.Show("Inicio de sesión de administrador exitoso");
            }
            else
            {
                MessageBox.Show("Nombre de usuario o contraseña de administrador incorrectos");
                txtUsuario.Clear();
                txtContraseña.Clear();
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            negocio = new TiendaNegocio();

            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            negocio.RegistrarAdmin(usuario, contraseña);
            MessageBox.Show("Administrador registrado.");

            txtUsuario.Clear();
            txtContraseña.Clear();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
