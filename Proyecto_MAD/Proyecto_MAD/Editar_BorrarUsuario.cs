using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Proyecto_MAD
{
    public partial class Editar_BorrarUsuario : Form
    {

        public Editar_BorrarUsuario()
        {
            InitializeComponent();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new InicioSesion();
            pantalla.Show();
        }

        private void busquedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Busqueda();
            pantalla.Show();
        }

        private void consultarBibliaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new ConsultarBiblia();
            pantalla.Show();
        }

        private void favoritosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Favoritos();
            pantalla.Show();
        }

        private void historialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new HistorialBusqueda();
            pantalla.Show();
        }

        private void preferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Preferencias();
            pantalla.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new AgregarUsuario();
            pantalla.Show();
        }

        private void EDIT_DELETEUser_Load(object sender, EventArgs e)
        {
            Usuario us = InicioSesion.UsuarioEnSesion;

            textBox1.Text = us.email;
            textBox2.Text = us.contrasena;
            textBox3.Text = us.nombre;
            textBox4.Text = us.apellido1;
            textBox5.Text = us.apellido2;
            dateTimePicker1.Value = us.fechaNac;
            
            if(us.idgenero == 1)
            {
                comboBox1.SelectedIndex = 1;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string nom = textBox3.Text;
            string ape1 = textBox4.Text;
            string ape2 = textBox5.Text;
            DateTime fech = dateTimePicker1.Value;

            EnlaceDB enlaceDB = new EnlaceDB();
            if(enlaceDB.UpdateUsuarios(email, nom, ape1, ape2, fech))
            {
                MessageBox.Show("Usuario actualizado exitosamente");
            }
            else
            {
                MessageBox.Show("Ha habido un error");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string email= textBox1.Text;

            EnlaceDB enlaceDB = new EnlaceDB();
            if (enlaceDB.DeleteUsuarios(email))
            {
                MessageBox.Show("El usuario se ha dado de baja");
            }
            else
            {
                MessageBox.Show("Ha habido un error");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new CambioContraseña();
            pantalla.Show();
        }
    }
    
}
