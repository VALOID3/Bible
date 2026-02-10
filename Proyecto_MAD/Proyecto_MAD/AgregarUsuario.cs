using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Proyecto_MAD
{
    public partial class AgregarUsuario : Form
    {
        public AgregarUsuario()
        {
            InitializeComponent();
        }

        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string nom = textBox3.Text;
            string ape1 = textBox4.Text;
            string ape2 = textBox5.Text;
            DateTime fech = dateTimePicker1.Value;
            //Primero obtenemos el texto del combobox
            string strgen = comboBox1.Text;
            //Y después, de acuerdo a ese texto, le asignamos el Id de acuerdo al género
            int idgen;

            if (strgen.Equals("Masculino"))
            {
                idgen = 1;
            }
            else {
                idgen = 2;
            }

            string contra = textBox2.Text;

            EnlaceDB enlaceDB = new EnlaceDB();


            if (ValidarEmail(email))
            {

                if (ValidarContra(contra))
                {

                    if (enlaceDB.InsertUsuarios(email, nom, ape1, ape2, fech, idgen, contra))
                    {
                        //Limpiamos los textBox
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        MessageBox.Show("Usuario agregado exitosamente");
                        this.Hide();
                        var pantalla = new InicioSesion();
                        pantalla.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ha habido un error");
                    }
                }
                else
                {
                    MessageBox.Show("la contraseña debe tener al menos 8 caracteres, una mayuscula, una minuscula y un caracter especial", "Error");
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese un correo valido", "Error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new InicioSesion();
            pantalla.Show();
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
        
        private bool ValidarEmail(string email)
        {
           EnlaceDB enlace = new EnlaceDB();

            Usuario User = enlace.ObtenerUsuario(email);

            if (string.IsNullOrEmpty(User.email))
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
