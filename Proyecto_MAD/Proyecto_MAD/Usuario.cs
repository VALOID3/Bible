using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_MAD
{
    public class Usuario
    {
        public string email {  get; set; }
        public string contrasena { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public DateTime fechaNac {  get; set; }
        public int idgenero { get; set; }
        public int idestatus {  get; set; }
    }
}
