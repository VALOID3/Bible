using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Proyecto_MAD
{
    public partial class CambioContraseña : Form
    {
        public CambioContraseña()
        {
            InitializeComponent();
        }

        private void CambioContraseña_Load(object sender, EventArgs e)
        {
            string usuario = InicioSesion.UsuarioEnSesion.email;

            EnlaceDB enlaceDB = new EnlaceDB();

            DataTable contrasenas = enlaceDB.ObtenerContra(usuario);

            textBox1.Text = contrasenas.Rows[1].Field<string>("contrasena");
            textBox3.Text = contrasenas.Rows[0].Field<string>("contrasena");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string usu = InicioSesion.UsuarioEnSesion.email; 
            string contranueva = textBox2.Text;
            string contraanterior = textBox1.Text;
            string contraactual = textBox3.Text;

            EnlaceDB enlaceDB = new EnlaceDB();

            if (contranueva.Equals(contraanterior ) || (contranueva.Equals(contraactual)))
            {
                MessageBox.Show("No puede ser igual a las contraseñas anteriores", "Error");
            }
            else
            {
                if (ValidarContra(contranueva))
                {
                    enlaceDB.CambiarContra(contranueva, usu);

                    if (InicioSesion.Contra_Temporal)
                    {
                        //Actualiza la bandera de contraseña temporal
                        InicioSesion.Contra_Temporal = false;
                        this.Hide();
                        var pantalla = new Preferencias();
                        pantalla.Show();
                    }
                    else
                    {
                        this.Hide();
                        var pantalla = new Editar_BorrarUsuario();
                        pantalla.Show();
                    }
                }
                else
                {
                    MessageBox.Show("la contraseña debe tener al menos 8 caracteres, una mayuscula, una minuscula y un caracter especial", "Error");
                }
                

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (InicioSesion.Contra_Temporal)
            {
                MessageBox.Show("Debe actualizar su contraseña", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Hide();
                var pantalla = new Editar_BorrarUsuario();
                pantalla.Show();
            }
        }

        private bool ValidarContra(string contrasena)
        {


            var TieneMayus = new Regex(@"[A-Z]+");
            var MinCaracter = new Regex(@".{8,}");
            var CaracterEspe = new Regex(@"[(¡#$%&/=’?¡¿:;,.-_+*{}]");

            if (TieneMayus.IsMatch(contrasena) && MinCaracter.IsMatch(contrasena) && CaracterEspe.IsMatch(contrasena))
            {

                return true;

            }
            else
            {
                return false;

            }
        }

    }
}
