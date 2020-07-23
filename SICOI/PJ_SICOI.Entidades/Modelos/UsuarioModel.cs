using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.Entidades.Modelos
{
    public class UsuarioModel
    {
        public string IDUsuario { get; set; }
        public string CodDespacho { get; set; }
        public string Contrasena { get; set; }
        public DateTime UltimoCambio { get; set; }
        public List<Rol> RolesUsuario { get; set; }
        public string TokenUsuario { get; set; }
        public class Rol
        {
            public string IDRol { get; set; }
            public string Nombre { get; set; }
        }
    }
}
