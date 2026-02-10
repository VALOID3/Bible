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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_MAD
{
    public partial class HistorialBusqueda : Form
    {
        public HistorialBusqueda()
        {
            InitializeComponent();
        }

        private void btn_aceptar_Click(object sender, EventArgs e)
        {
            string usuario = InicioSesion.UsuarioEnSesion.email;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable historial = enlaceDB.consultHisto(usuario);

            dataGridView1.DataSource = historial;
            //Para ocultar el id al usuario
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].HeaderText = "IDIOMA";
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].HeaderText = "VERSION";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "TESTAMENTO";
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].HeaderText = "LIBRO";
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].HeaderText = "PALABRA(S)";
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].HeaderText = "FECHA";
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].HeaderText = "¿HUBO RESULTADOS?";
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void HistorialBusqueda_Load(object sender, EventArgs e)
        {
            string usuario = InicioSesion.UsuarioEnSesion.email;
            EnlaceDB enlaceDB = new EnlaceDB();
            DataTable historial = enlaceDB.consultHisto(usuario);

            dataGridView1.DataSource = historial;
            //Para ocultar el id al usuario
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].HeaderText = "IDIOMA";
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].HeaderText = "VERSION";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].HeaderText = "TESTAMENTO";
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[4].HeaderText = "LIBRO";
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[5].HeaderText = "PALABRA(S)";
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[6].HeaderText = "FECHA";
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[7].HeaderText = "¿HUBO RESULTADOS?";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = InicioSesion.UsuarioEnSesion.email;

            EnlaceDB enlaceDB = new EnlaceDB();
            if (enlaceDB.DeleteAllHisto(usuario))
            {
                MessageBox.Show("Se ha borrado su historial", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int seleccionado = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (seleccionado > 0)
            {
                string usuario = InicioSesion.UsuarioEnSesion.email;
                short idbus = short.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                EnlaceDB enlaceDB = new EnlaceDB();
                if(enlaceDB.DeleteHisto(usuario, idbus))
                {
                    MessageBox.Show("Se ha borrado la busqueda seleccionada de su historial", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
