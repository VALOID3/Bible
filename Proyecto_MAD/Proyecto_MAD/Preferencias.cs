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
    public partial class Preferencias : Form
    {
        //Variables globales para guardar las preferencias elegidas
        public static Idioma idioma_elegido;
        public static Version version_elegida;

        public Preferencias()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stridioma = comboBox1.Text;
            string strversion = comboBox2.Text;

            //Primero validamos que hayan seleccionado algo
            if (string.IsNullOrEmpty(stridioma) || string.IsNullOrEmpty(strversion))
            {
                MessageBox.Show("Debe elegir algo primero");
            }
            else
            {
                //Y guardamos las preferencias
                idioma_elegido = new Idioma();
                idioma_elegido.id = Convert.ToInt16(comboBox1.SelectedValue);
                idioma_elegido.nombre = comboBox1.Text;
                version_elegida = new Version();    
                version_elegida.id = Convert.ToInt16(comboBox2.SelectedValue);
                version_elegida.nombre = comboBox2.Text;

                this.Hide();
                var pantalla = new ConsultarBiblia();
                pantalla.Show();
            }
        }

        private void Preferencias_Load(object sender, EventArgs e)
        {
            //Al cargar la ventana se inicializa el combobox para mostrar los idiomas
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaIdiomas = enlaceDB.GetIdiomas();
            comboBox1.DisplayMember = ListaIdiomas.Columns[1].ColumnName;
            comboBox1.ValueMember = ListaIdiomas.Columns[0].ColumnName;
            comboBox1.DataSource = ListaIdiomas;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new InicioSesion();
            pantalla.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cada vez que cambias de idioma, el combox de versiones se actualiza
            short idioma = Convert.ToInt16(comboBox1.SelectedValue);
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaVersiones = enlaceDB.GetVersiones(idioma);
            comboBox2.DisplayMember = ListaVersiones.Columns[1].ColumnName;
            comboBox2.ValueMember = ListaVersiones.Columns[0].ColumnName;
            comboBox2.DataSource = ListaVersiones;
        }
    }
}
