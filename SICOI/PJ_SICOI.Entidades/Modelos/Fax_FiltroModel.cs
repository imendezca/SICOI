using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.Entidades.Modelos
{
    public class Fax_FiltroModel
    {
        public string Asunto { get; set; }
        public string CodDespacho { get; set; }
        public string ConsecutivoFax { get; set; }
        public string Expediente { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? IDPrioridad { get; set; }
    }
}
