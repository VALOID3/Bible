using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Proyecto_MAD
{
    public partial class ConsultarBiblia : Form
    {
        public ConsultarBiblia()
        {
            InitializeComponent();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new InicioSesion();
            pantalla.Show();
        }
        
        private void ConsultarBiblia_Load(object sender, EventArgs e)
        {
            //Cargamos el combobox de testamento de acuerdo a las preferencias
            short idioma = Preferencias.idioma_elegido.id;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaTestamentos = enlaceDB.GetTestamentos(idioma);
            comboBox3.DisplayMember = ListaTestamentos.Columns[1].ColumnName;
            comboBox3.ValueMember = ListaTestamentos.Columns[0].ColumnName;
            comboBox3.DataSource = ListaTestamentos;
        }

        private void favoritosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Favoritos();
            pantalla.Show();
        }

        private void historialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this .Hide();
            var pantalla = new HistorialBusqueda();
            pantalla.Show();
        }

        private void busquedaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Busqueda();
            pantalla.Show();
        }

        private void gestionarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Editar_BorrarUsuario();
            pantalla.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short version = Preferencias.version_elegida.id;
            short libro = Convert.ToInt16(comboBox4.SelectedValue);
            byte capitulo;
            byte versiculo;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable Pasajes;
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("No ha ingresado ningún número de capítulo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                capitulo = byte.Parse(comboBox1.Text);
                
                //Si el usuario ingreso un número de versículo, lo agregamos a los parametros
                if(!string.IsNullOrEmpty(comboBox2.Text))
                {
                    versiculo = byte.Parse(comboBox2.Text);
                }
                else
                {
                    //De otra manera, lo igualamos a cero, por lo que se consultara el capitulo entero
                    versiculo = 0;
                }

                Pasajes = enlaceDB.ConsultarCapituloBiblia(version, libro, capitulo);
                //Cargamos los pasajes al datagrid
                dataGridView1.DataSource = Pasajes;
                //Ajustamos el tamaño del datagrid a los pasajes
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].HeaderText = "LIBRO";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].HeaderText = "CAPITULO";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].HeaderText = "VERSICULO";
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].HeaderText = "PASAJE";

            }
        }

        private void preferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new Preferencias();
            pantalla.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int seleccionado = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if( seleccionado > 0)
            {
                string usuario = InicioSesion.UsuarioEnSesion.email;
                short idioma = Preferencias.idioma_elegido.id;
                short testamento = Convert.ToInt16(comboBox3.SelectedValue);
                short version = Preferencias.version_elegida.id;
                short libro = Convert.ToInt16(dataGridView1.CurrentRow.Cells[1].Value);
                byte numCap = Convert.ToByte(dataGridView1.CurrentRow.Cells[3].Value);
                byte numVers = Convert.ToByte(dataGridView1.CurrentRow.Cells[4].Value);

                EnlaceDB enlaceDB = new EnlaceDB();
                if(enlaceDB.InsertFavoritos(idioma, testamento, version, libro, numCap, numVers, usuario))
                {
                    MessageBox.Show("El pasaje se ha agregado a favoritos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                  
            }
            else
            {
                MessageBox.Show("No ha seleccionado ningun campo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer voz = new SpeechSynthesizer();
            voz.SetOutputToDefaultAudioDevice();
            voz.Speak(dataGridView1.CurrentCell.Value.ToString());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargamos el combobox de libros de acuerdo a las preferencias
            short idioma = Preferencias.idioma_elegido.id;
            short testamento = Convert.ToInt16(comboBox3.SelectedValue);
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable ListaLibros = enlaceDB.GetLibros(idioma, testamento);
            comboBox4.DisplayMember = ListaLibros.Columns[1].ColumnName;
            comboBox4.ValueMember = ListaLibros.Columns[0].ColumnName;
            comboBox4.DataSource = ListaLibros;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable Capitulos = enlaceDB.GetCapitulos(Convert.ToInt16(comboBox4.SelectedValue));
            int totalcap = Capitulos.Rows[0].Field<byte>("CapitulosTot");
            comboBox1.Items.Clear();
            for (int i = 0; i < totalcap; i++)
            {
                int numCap = i + 1;
                comboBox1.Items.Add(numCap.ToString());
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnlaceDB enlaceDB = new EnlaceDB();
            short libro = Convert.ToInt16(comboBox4.SelectedValue);
            byte numCap = Convert.ToByte(comboBox1.Text);
            DataTable Versiculos = enlaceDB.GetVersiculos(libro, numCap);
            int totalvers = Versiculos.Rows[0].Field<int>("TotalVers");
            comboBox2.Items.Clear();
            for(int i = 0; i < totalvers; i++)
            {
                int numvers = i + 1;
                comboBox2.Items.Add(numvers.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            short version = Preferencias.version_elegida.id;
            short libro = Convert.ToInt16(comboBox4.SelectedValue);
            byte capitulo;
            byte versiculo;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable Pasajes;
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("No ha ingresado ningún número de capítulo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                capitulo = byte.Parse(comboBox1.Text);

                //Si el usuario ingreso un número de versículo, lo agregamos a los parametros
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    versiculo = byte.Parse(comboBox2.Text);
                }
                else
                {
                    //De otra manera, lo igualamos a cero, por lo que se consultara el capitulo entero
                    versiculo = 0;
                }

                Pasajes = enlaceDB.ConsultarVersiculoBiblia(version, libro, capitulo, versiculo);
                //Cargamos los pasajes al datagrid
                dataGridView1.DataSource = Pasajes;
                //Ajustamos el tamaño del datagrid a los pasajes
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[2].HeaderText = "LIBRO";
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[3].HeaderText = "CAPITULO";
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[4].HeaderText = "VERSICULO";
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns[5].HeaderText = "PASAJE";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Copiar pasaje al portapapeles
            int seleccionado = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if(seleccionado > 0)
            {
                string pasaje = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                Clipboard.SetText(pasaje);
                MessageBox.Show("Se copio el pasaje a su portapapeles", "Aviso");
            }
            else
            {
                MessageBox.Show("No hay ningun pasaje seleccionado", "Error");
            }
        }
    }
}
