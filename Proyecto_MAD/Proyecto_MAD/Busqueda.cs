using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using WindowsFormsApplication1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_MAD
{
    public partial class Busqueda : Form
    {
        public Busqueda()
        {
            InitializeComponent();
        }

        private void busqueda_Load(object sender, EventArgs e)
        {
            short idioma = Preferencias.idioma_elegido.id;
         

            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaTestamentos = enlaceDB.GetTestamentos(idioma);
            comboBox1.DisplayMember = ListaTestamentos.Columns[1].ColumnName;
            comboBox1.ValueMember = ListaTestamentos.Columns[0].ColumnName;
            comboBox1.DataSource = ListaTestamentos;
        
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new InicioSesion();
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

        private void button1_Click(object sender, EventArgs e)
        {
            short version = Preferencias.version_elegida.id;
            string palabra = textBox1.Text;
            short libro = Convert.ToInt16(comboBox2.SelectedValue);

            //Aqui limitamos la busqueda por el libro y testamento elegido
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable Pasajes = enlaceDB.BusquedaPasajes(version, libro, palabra);

            //Limpia el textBox
            textBox1.Text = "";

            dataGridView1.DataSource = Pasajes;
            //Ajustamos el tamaño del datagrid a los pasajes
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[0].HeaderText = "LIBRO";
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].HeaderText = "CAPITULO";
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].HeaderText = "VERSICULO";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "PASAJE";

            //Esta parte es para guardar la busqueda hecha
            string usuario = InicioSesion.UsuarioEnSesion.email;
            short idioma = Preferencias.idioma_elegido.id;
            short testamento = Convert.ToInt16(comboBox1.SelectedValue);
            //Un if para revisar si hubo resultados
            if (Pasajes.Rows.Count > 0)
            {
                enlaceDB.savebusquedaLimitada(palabra, idioma, version, testamento, libro, true, usuario);
            }
            else
            {
                enlaceDB.savebusquedaLimitada(palabra, idioma, version, testamento, libro, false, usuario);
            }

        }

        private void gestionarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Editar_BorrarUsuario();
            pantalla.Show();
        }

        private void consultarBibliaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new ConsultarBiblia();
            pantalla.Show();
        }

        private void preferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Preferencias();
            pantalla.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            short version = Preferencias.version_elegida.id;
            string palabra = textBox1.Text;

            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable Pasajes = enlaceDB.BusquedaPasajes(version, 0, palabra);

            //Limpia el textBox
            textBox1.Text = "";

            dataGridView1.DataSource = Pasajes;
            //Ajustamos el tamaño del datagrid a los pasajes
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[0].HeaderText = "LIBRO";
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].HeaderText = "CAPITULO";
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].HeaderText = "VERSICULO";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "PASAJE";

            //Esta parte es para guardar la busqueda hecha
            string usuario = InicioSesion.UsuarioEnSesion.email;
            short idioma = Preferencias.idioma_elegido.id;
            //Un if para revisar si hubo resultados
            if (Pasajes.Rows.Count > 0) {
                enlaceDB.savebusquedaBiblia(palabra, idioma, version, true, usuario);
            }
            else
            {
                enlaceDB.savebusquedaBiblia(palabra, idioma, version, false, usuario);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            short idioma = Preferencias.idioma_elegido.id;

            short testamento = Convert.ToInt16(comboBox1.SelectedValue);
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaLibros = enlaceDB.GetLibros(idioma, testamento);
            comboBox2.DisplayMember = ListaLibros.Columns[1].ColumnName;
            comboBox2.ValueMember = ListaLibros.Columns[0].ColumnName;
            comboBox2.DataSource = ListaLibros;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer voz = new SpeechSynthesizer();
            voz.SetOutputToDefaultAudioDevice();
            voz.Speak(dataGridView1.CurrentCell.Value.ToString());
        }
    }
}
