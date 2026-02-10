using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Proyecto_MAD
{
    public partial class InicioSesion : Form
    {
        //Variable global para guardar el usuario que ingreso
        public static Usuario UsuarioEnSesion;
        public static bool Contra_Temporal = false;
        //Variable para guardar la cantidad de intentos al tratar de ingresar
        int intentosFallidos = 0;
        public InicioSesion()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new AgregarUsuario();
            pantalla.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ema = textBox1.Text;
            string contra = textBox2.Text;

            EnlaceDB enlaceDB = new EnlaceDB();

            if (enlaceDB.Autentificar(ema, contra))
            {
                UsuarioEnSesion = enlaceDB.ObtenerUsuario(ema);
                if (Contra_Temporal)
                {
                    this.Hide();
                    var pantalla = new CambioContraseña();
                    pantalla.Show();
                }
                else
                {
                    this.Hide();
                    var pantalla = new Preferencias();
                    pantalla.Show();
                }
            }
            else
            {
                textBox2.Text = "";
                MessageBox.Show("Correo y/o contraseña incorrectos");
                intentosFallidos++;
                if (intentosFallidos >= 3)
                {
                    enlaceDB.ActualizarEstatus(ema, "Inhabilitado");
                    MessageBox.Show("Su usuario ha sido inhabilitado", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ema = textBox1.Text;
            EnlaceDB enlaceDB = new EnlaceDB();
            Usuario us = enlaceDB.ObtenerUsuario(ema);
            if (string.IsNullOrEmpty(us.email))
            {
                MessageBox.Show("Usuario no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (enlaceDB.ActualizarEstatus(ema, "Activo"))
                {
                    enlaceDB.GenerarContraTemporal(us.email);
                    DataTable contras = enlaceDB.ObtenerContra(us.email);
                    string contraTemporal = contras.Rows[0].Field<string>("contrasena");
                    Clipboard.SetText(contraTemporal);
                    Contra_Temporal = true;
                    MessageBox.Show("Su usuario ha sido reactivado y su contraseña temporal está en su portapapeles", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo reactivar su usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
