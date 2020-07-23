using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.Entidades.Modelos
{
    public class BitacoraModel
    {
        public int ID { get; set; }
        public DateTime FechaHora { get; set; }
        public string IDUsuario { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public string Pantalla { get; set; }
    }
}
