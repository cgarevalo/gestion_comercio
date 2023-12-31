using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicacion
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin login = new frmLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                // Si las credenciales son válidas, muestra la pantalla principal
                // Para que cargue Principal, al botón inicio del login, lo puse en DialogResult en OK
                Application.Run(new Principal());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
