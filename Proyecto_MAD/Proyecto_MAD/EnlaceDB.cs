
        /*
Autor: Alejandro Villarreal

LMAD

PARA EL PROYECTO ES OBLIGATORIO EL USO DE ESTA CLASE, 
EN EL SENTIDO DE QUE LOS DATOS DE CONEXION AL SERVIDOR ESTAN DEFINIDOS EN EL App.Config
Y NO TENER ESOS DATOS EN CODIGO DURO DEL PROYECTO.

NO SE PERMITE HARDCODE.

LOS MÉTODOS QUE SE DEFINEN EN ESTA CLASE SON EJEMPLOS, PARA QUE SE BASEN Y USTEDES HAGAN LOS SUYOS PROPIOS
Y DEFINAN Y PROGRAMEN TODOS LOS MÉTODOS QUE SEAN NECESARIOS PARA SU PROYECTO.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Proyecto_MAD;


/*
Se tiene que cambiar el namespace para el que usen en su proyecto
*/
namespace WindowsFormsApplication1
    {
        public class EnlaceDB
        {
            static private string _aux { set; get; }
            static private SqlConnection _conexion;
            static private SqlDataAdapter _adaptador = new SqlDataAdapter();
            static private SqlCommand _comandosql = new SqlCommand();
            static private DataTable _tabla = new DataTable();
            static private DataSet _DS = new DataSet();

            public DataTable obtenertabla
            {
                get
                {
                    return _tabla;
                }
            }

            private static void conectar()
            {
                /*
                Para que funcione el ConfigurationManager
                en la sección de "Referencias" de su proyecto, en el "Solution Explorer"
                dar clic al botón derecho del mouse y dar clic a "Add Reference"
                Luego elegir la opción System.Configuration

                tal como lo vimos en clase.
                */
                string cnn = ConfigurationManager.ConnectionStrings["Grupo03"].ToString();
                // Cambiar Grupo01 por el que ustedes hayan definido en el App.Confif
                _conexion = new SqlConnection(cnn);
                _conexion.Open();
            }
            private static void desconectar()
            {
                _conexion.Close();
            }

        ////////////// METODO PARA LOGIN DE USUARIO /////////////////
        public bool Autentificar(string us, string ps)
            {
                bool isValid = false;
                try
                {
                    conectar();
                    string qry = "spGestionUsuarios";
                    _comandosql = new SqlCommand(qry, _conexion);
                    _comandosql.CommandType = CommandType.StoredProcedure;
                    _comandosql.CommandTimeout = 9000;

                    var parametro1 = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                    parametro1.Value = "L";
                    var parametro2 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                    parametro2.Value = us;
                    var parametro3 = _comandosql.Parameters.Add("@contrasena", SqlDbType.VarChar, 40);
                    parametro3.Value = ps;

                    _adaptador.SelectCommand = _comandosql;
                    _tabla.Clear();
                    _adaptador.Fill(_tabla);

                    if (_tabla.Rows.Count > 0)
                    {
                        isValid = true;
                    }

                }
                catch (SqlException e)
                {
                    isValid = false;
                }
                finally
                {
                    desconectar();
                }

                return isValid;
            }

        //////////////////// METODO PARA OBTENER UN USUARIO ///////////////////////
        public Usuario ObtenerUsuario(string us)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            Usuario usuario = new Usuario();
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 9000;

                var parametro1 = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro1.Value = "O";
                var parametro2 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro2.Value = us;

                _adaptador.SelectCommand = _comandosql;
                _tabla.Clear();
                _adaptador.Fill(tabla);

                if (tabla.Rows.Count > 0)
                {
                    usuario.email = tabla.Rows[0].Field<string>("email");
                    usuario.contrasena = tabla.Rows[0].Field<string>("contrasena");
                    usuario.nombre = tabla.Rows[0].Field<string>("nombre");
                    usuario.apellido1 = tabla.Rows[0].Field<string>("apellido1");
                    usuario.apellido2 = tabla.Rows[0].Field<string>("apellido2");
                    usuario.fechaNac = tabla.Rows[0].Field<DateTime>("fechaNac");
                    usuario.idgenero = tabla.Rows[0].Field<short>("idGenero");
                    usuario.idestatus = tabla.Rows[0].Field<short>("IdEstatus");
                }

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return usuario;
        }

        //////////////////// METODO INSERT USUARIOS ///////////////////////
        public bool InsertUsuarios(string ema, string nom, string ape1, string ape2, DateTime fech, int idgen, string contra)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "I";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = ema;
                var parametro2 = _comandosql.Parameters.Add("@nombre", SqlDbType.VarChar, 20);
                parametro2.Value = nom;
                var parametro3 = _comandosql.Parameters.Add("@apellido1", SqlDbType.VarChar, 20);
                parametro3.Value = ape1;
                var parametro4 = _comandosql.Parameters.Add("@apellido2", SqlDbType.VarChar, 20);
                parametro4.Value = ape2;
                var parametro5 = _comandosql.Parameters.Add("@fechaNac", SqlDbType.Date);
                parametro5.Value = fech;
                var parametro6 = _comandosql.Parameters.Add("@idGenero", SqlDbType.SmallInt);
                parametro6.Value = idgen;
                var parametro7 = _comandosql.Parameters.Add("@contrasena", SqlDbType.VarChar, 40);
                parametro7.Value = contra;




                _adaptador.InsertCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;

        }

        //////////////////// METODO PARA ACTUALIZAR EL USUARIO ///////////////////////
        public bool UpdateUsuarios(string ema, string nom, string ape1, string ape2, DateTime fech)
        {
            var msg = "";
            var update = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "U";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = ema;
                var parametro2 = _comandosql.Parameters.Add("@nombre", SqlDbType.VarChar, 20);
                parametro2.Value = nom;
                var parametro3 = _comandosql.Parameters.Add("@apellido1", SqlDbType.VarChar, 20);
                parametro3.Value = ape1;
                var parametro4 = _comandosql.Parameters.Add("@apellido2", SqlDbType.VarChar, 20);
                parametro4.Value = ape2;
                var parametro5 = _comandosql.Parameters.Add("@fechaNac", SqlDbType.Date);
                parametro5.Value = fech;
                

                _adaptador.UpdateCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                update = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return update;
        }

        //METODO PARA CAMBIO DE CONTRASEÑA//
        public bool CambiarContra(string contra, string user)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "C";
                var parametro1 = _comandosql.Parameters.Add("@contrasena", SqlDbType.VarChar, 40);
                parametro1.Value = contra;
                var parametro2 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro2.Value = user;

                _adaptador.UpdateCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        //METODO PARA RECUPERACIÓN DE CONTRASEÑA//
        public bool GenerarContraTemporal(string email)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "T";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;

                _adaptador.UpdateCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        //METODO PARA OBTENER LAS CONTRASEÑAS//
        public DataTable ObtenerContra(string user)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "P";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = user;


                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }


        //////////////////// METODO PARA BAJA LÓGICA DE USUARIO ///////////////////////
        public bool DeleteUsuarios(string ema)
        {
            var msg = "";
            var delete = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "B";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = ema;
                

                _adaptador.UpdateCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                delete = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return delete;
        }

        //////////////////// METODO PARA ACTUALIZAR ESTATUS DE USUARIO ///////////////////////
        public bool ActualizarEstatus(string email, string estatusNuevo)
        {
            var msg = "";
            var updateE = true;
            try
            {
                conectar();
                string qry = "spGestionUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "E";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;
                var parametro2 = _comandosql.Parameters.Add("@estatusNuevo", SqlDbType.VarChar, 15);
                parametro2.Value = estatusNuevo;


                _adaptador.UpdateCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                updateE = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return updateE;
        }

        //////////////////// METODO PARA OBTENER LOS IDIOMAS EN LA BASE DE DATOS ///////////////////////
        public DataTable GetIdiomas()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaIdiomas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;


                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA OBTENER LAS VERSIONES EN LA BASE DE DATOS ///////////////////////
        public DataTable GetVersiones(short idioma)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaVersiones";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Idioma", SqlDbType.SmallInt);
                parametro1.Value = idioma;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA OBTENER LOS TESTAMENTOS EN LA BASE DE DATOS ///////////////////////
        public DataTable GetTestamentos(short idioma)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaTestamentos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Idioma", SqlDbType.SmallInt);
                parametro1.Value = idioma;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA OBTENER LOS LIBROS DE ACUERDO A LAS PREFERENCIAS ///////////////////////
        public DataTable GetLibros(short idioma, short testamento)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaLibros";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Idioma", SqlDbType.SmallInt);
                parametro1.Value = idioma;
                var parametro2 = _comandosql.Parameters.Add("@Testamento", SqlDbType.SmallInt);
                parametro2.Value = testamento;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA OBTENER EL NUMERO DE CAPITULOS DE UN LIBRO ///////////////////////
        public DataTable GetCapitulos(short libro)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaCapitulos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Libro", SqlDbType.SmallInt);
                parametro1.Value = libro;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA OBTENER EL NUMERO DE VERSICULOS DE UN CAPITULO ///////////////////////
        public DataTable GetVersiculos(short libro, byte numCap)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spNumerodeVersiculos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Libro", SqlDbType.SmallInt);
                parametro1.Value = libro;
                var parametro2 = _comandosql.Parameters.Add("@NumCap", SqlDbType.TinyInt);
                parametro2.Value = numCap;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO PARA CONSULTAR LA BIBLIA DE ACUERDO A LAS PREFERENCIAS ///////////////////////
        public DataTable ConsultarCapituloBiblia(short version, short libro, byte capitulo)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaBiblia";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Version", SqlDbType.SmallInt);
                parametro1.Value = version;
                var parametro2 = _comandosql.Parameters.Add("@Libro", SqlDbType.SmallInt);
                parametro2.Value = libro;
                var parametro3 = _comandosql.Parameters.Add("@NumeroCapitulo", SqlDbType.TinyInt);
                parametro3.Value = capitulo;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        public DataTable ConsultarVersiculoBiblia(short version, short libro, byte capitulo, byte versiculo)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spConsultaBiblia";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro1 = _comandosql.Parameters.Add("@Version", SqlDbType.SmallInt);
                parametro1.Value = version;
                var parametro2 = _comandosql.Parameters.Add("@Libro", SqlDbType.SmallInt);
                parametro2.Value = libro;
                var parametro3 = _comandosql.Parameters.Add("@NumeroCapitulo", SqlDbType.TinyInt);
                parametro3.Value = capitulo;
                var parametro4 = _comandosql.Parameters.Add("@NumeroVersiculo", SqlDbType.TinyInt);
                parametro4.Value = versiculo;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO INSERTAR FAVORITOS ///////////////////////
        public bool InsertFavoritos(short idioma, short testamento, short version, short libro, byte numCap,
                                    byte numVers, string email)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionFavoritos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "I";
                var parametro1 = _comandosql.Parameters.Add("@idioma", SqlDbType.SmallInt);
                parametro1.Value = idioma;
                var parametro2 = _comandosql.Parameters.Add("@testamento", SqlDbType.SmallInt);
                parametro2.Value = testamento;
                var parametro3 = _comandosql.Parameters.Add("@version", SqlDbType.SmallInt);
                parametro3.Value = version;
                var parametro4 = _comandosql.Parameters.Add("@libro", SqlDbType.SmallInt);
                parametro4.Value = libro;
                var parametro5 = _comandosql.Parameters.Add("@numeroCap", SqlDbType.TinyInt);
                parametro5.Value = numCap;
                var parametro6 = _comandosql.Parameters.Add("@numeroVers", SqlDbType.TinyInt);
                parametro6.Value = numVers;
                var parametro7 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro7.Value = email;

                _adaptador.InsertCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        //////////////////// METODO CONSULTAR FAVORITOS ///////////////////////
        public DataTable ConsultFavoritos(short idioma, short version, string email)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spGestionFavoritos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "C";
                var parametro2 = _comandosql.Parameters.Add("@idioma", SqlDbType.SmallInt);
                parametro2.Value = idioma;
                var parametro3 = _comandosql.Parameters.Add("@version", SqlDbType.SmallInt);
                parametro3.Value = version;
                var parametro4 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro4.Value = email;


                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO ELIMINAR TODOS LOS FAVORITOS ///////////////////////
        public bool DeleteAllFav(string email)
        {
            var msg = "";
            var delete = true;
            try
            {
                conectar();
                string qry = "spGestionFavoritos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "B";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;
               

                _adaptador.DeleteCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                delete = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return delete;
        }

        //////////////////// METODO ELIMINAR UN FAVORITO ///////////////////////
        public bool DeleteFav(short idfav, string email)
        {
            var msg = "";
            var delete = true;
            try
            {
                conectar();
                string qry = "spGestionFavoritos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "U";
                var parametro1 = _comandosql.Parameters.Add("@id", SqlDbType.SmallInt);
                parametro1.Value = idfav;
                var parametro2 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro2.Value = email;

                _adaptador.DeleteCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                delete = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return delete;
        }


        //////////////////// METODO PARA BUSQUEDA DE PASAJES BIBLICOS ///////////////////////
        public DataTable BusquedaPasajes(short version, short libro, string palabra)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "B";
                var parametro1 = _comandosql.Parameters.Add("@version", SqlDbType.SmallInt);
                parametro1.Value = version;

                if (libro != 0)
                {
                    var parametr2 = _comandosql.Parameters.Add("@libro", SqlDbType.SmallInt);
                    parametr2.Value = libro;
                }

                var parametro3 = _comandosql.Parameters.Add("@palabra", SqlDbType.VarChar, 30);
                parametro3.Value = palabra;


                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO GUARDAR BUSQUEDA ///////////////////////
        public bool savebusquedaBiblia(string palabra, short idioma, short version, bool huboresultados, 
                                        string usuario)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "G";
                var parametro1 = _comandosql.Parameters.Add("@palabra", SqlDbType.VarChar, 30);
                parametro1.Value = palabra;
                var parametro2 = _comandosql.Parameters.Add("@idioma", SqlDbType.SmallInt);
                parametro2.Value = idioma;
                var parametro3 = _comandosql.Parameters.Add("@version", SqlDbType.SmallInt);
                parametro3.Value = version;
                var parametro4 = _comandosql.Parameters.Add("@huboresultados", SqlDbType.Bit);
                parametro4.Value = huboresultados;
                var parametro5 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro5.Value = usuario;

                _adaptador.InsertCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        public bool savebusquedaLimitada(string palabra, short idioma, short version, short testamento,
                                        short libro, bool huboresultados, string usuario)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "G";
                var parametro1 = _comandosql.Parameters.Add("@palabra", SqlDbType.VarChar, 30);
                parametro1.Value = palabra;
                var parametro2 = _comandosql.Parameters.Add("@idioma", SqlDbType.SmallInt);
                parametro2.Value = idioma;
                var parametro3 = _comandosql.Parameters.Add("@version", SqlDbType.SmallInt);
                parametro3.Value = version;
                var parametro4 = _comandosql.Parameters.Add("@testamento", SqlDbType.SmallInt);
                parametro4.Value = testamento;
                var parametro5 = _comandosql.Parameters.Add("@libro", SqlDbType.SmallInt);
                parametro5.Value = libro;
                var parametro6 = _comandosql.Parameters.Add("@huboresultados", SqlDbType.Bit);
                parametro6.Value = huboresultados;
                var parametro7 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro7.Value = usuario;

                _adaptador.InsertCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        //////////////////// METODO CONSULTAR HISTORIAL ///////////////////////
        public DataTable consultHisto(string email)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "H";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;


                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
                // la ejecución del SP espera que regrese datos en formato tabla

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }

        //////////////////// METODO ELIMINAR TODO EL HISTORIAL///////////////////////
        public bool DeleteAllHisto(string email)
        {
            var msg = "";
            var delete = true;
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "D";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;


                _adaptador.DeleteCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                delete = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return delete;
        }

        //////////////////// METODO ELIMINAR UNA BUSQUEDA DEL HISTORIAL ///////////////////////
        public bool DeleteHisto(string email, short idbus)
        {
            var msg = "";
            var delete = true;
            try
            {
                conectar();
                string qry = "spGestionBusquedas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var parametro = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                parametro.Value = "U";
                var parametro1 = _comandosql.Parameters.Add("@email", SqlDbType.VarChar, 40);
                parametro1.Value = email;
                var parametro2 = _comandosql.Parameters.Add("@id_busqueda", SqlDbType.SmallInt);
                parametro2.Value = idbus;

                _adaptador.DeleteCommand = _comandosql;
                // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                _comandosql.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                delete = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return delete;
        }

        public DataTable get_Users()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                // Ejemplo de cómo ejecutar un query, 
                // PERO lo correcto es siempre usar SP para cualquier consulta a la base de datos
                string qry = "Select Nombre, email, Fecha_modif from Usuarios where Activo = 0;";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.Text;
                // Esta opción solo la podrían utilizar si hacen un EXEC al SP concatenando los parámetros.
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }


        // Ejemplo de método para recibir una consulta en forma de tabla
        // Cuando el SP ejecutará un SELECT
        public DataTable get_Deptos(string opc)
            {
                var msg = "";
                DataTable tabla = new DataTable();
                try
                {
                    conectar();
                    string qry = "sp_Gestiona_Deptos";
                    _comandosql = new SqlCommand(qry, _conexion);
                    _comandosql.CommandType = CommandType.StoredProcedure;
                    _comandosql.CommandTimeout = 1200;

                    var parametro1 = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                    parametro1.Value = opc;


                    _adaptador.SelectCommand = _comandosql;
                    _adaptador.Fill(tabla);
                    // la ejecución del SP espera que regrese datos en formato tabla

                }
                catch (SqlException e)
                {
                    msg = "Excepción de base de datos: \n";
                    msg += e.Message;
                    MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    desconectar();
                }

                return tabla;
            }

            // Ejemplo de método para ejecutar un SP que no se espera que regrese información, 
            // solo que ejecute ya sea un INSERT, UPDATE o DELETE
            public bool Add_Deptos(string opc, string depto)
            {
                var msg = "";
                var add = true;
                try
                {
                    conectar();
                    string qry = "sp_Gestiona_Deptos";
                    _comandosql = new SqlCommand(qry, _conexion);
                    _comandosql.CommandType = CommandType.StoredProcedure;
                    _comandosql.CommandTimeout = 1200;

                    var parametro1 = _comandosql.Parameters.Add("@Opc", SqlDbType.Char, 1);
                    parametro1.Value = opc;
                    var parametro2 = _comandosql.Parameters.Add("@Nombre", SqlDbType.VarChar, 20);
                    parametro2.Value = depto;

                    _adaptador.InsertCommand = _comandosql;
                    // También se tienen las propiedades del adaptador: UpdateCommand  y DeleteCommand

                    _comandosql.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    add = false;
                    msg = "Excepción de base de datos: \n";
                    msg += e.Message;
                    MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    desconectar();
                }

                return add;
            }

    }
    }


