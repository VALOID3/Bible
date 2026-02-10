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
    public partial class Favoritos : Form
    {
        public Favoritos()
        {
            InitializeComponent();
        }

        private void Favoritos_Load(object sender, EventArgs e)
        {
            short idioma = Preferencias.idioma_elegido.id;
            short version = Preferencias.version_elegida.id;
            string usuario = InicioSesion.UsuarioEnSesion.email;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable favo = enlaceDB.ConsultFavoritos(idioma, version, usuario);

            dataGridView1.DataSource = favo;
            //OCULTA ID FAV
            dataGridView1.Columns[0].Visible = false;
            //OCULTA ID VERSION
            dataGridView1.Columns[1].Visible = false;
            //OCULTA ID LIBRO
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "LIBRO";
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].HeaderText = "CAPITULO";
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].HeaderText = "VERSICULO";
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].HeaderText = "PASAJE";
        }

        private void hostorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var pantalla = new HistorialBusqueda();
            pantalla.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short idioma = Preferencias.idioma_elegido.id;
            short version = Preferencias.version_elegida.id;
            string usuario = InicioSesion.UsuarioEnSesion.email;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable favo = enlaceDB.ConsultFavoritos(idioma, version, usuario);

            dataGridView1.DataSource = favo;
            //OCULTA ID FAV
            dataGridView1.Columns[0].Visible = false;
            //OCULTA ID VERSION
            dataGridView1.Columns[1].Visible = false;
            //OCULTA ID LIBRO
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "LIBRO";
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].HeaderText = "CAPITULO";
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].HeaderText = "VERSICULO";
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].HeaderText = "PASAJE";
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            int seleccionado = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (seleccionado > 0)
            {
                string usuario = InicioSesion.UsuarioEnSesion.email;
                short idfa = short.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                EnlaceDB enlaceDB = new EnlaceDB();
                if(enlaceDB.DeleteFav(idfa, usuario))
                {
                    MessageBox.Show("Se ha borrado el favorito seleccionado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("No ha seleccionado ningun campo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            string usuario = InicioSesion.UsuarioEnSesion.email;

            EnlaceDB enlaceDB = new EnlaceDB();
            if (enlaceDB.DeleteAllFav(usuario))
            {
                MessageBox.Show("Se han borrado todos sus favoritos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer voz = new SpeechSynthesizer();
            voz.SetOutputToDefaultAudioDevice();
            voz.Speak(dataGridView1.CurrentCell.Value.ToString());
        }
    }
}
