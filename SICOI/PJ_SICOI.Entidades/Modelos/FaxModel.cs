using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.Entidades.Modelos
{
    public class FaxModel
    {
        public string Periodo { get; set; }
        public string CodDespacho { get; set; }
        public int ConsFax { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaHoraIngreso { get; set; }
        public string Expediente { get; set; }
        public string Tipo { get; set; }
        public int CantFolios { get; set; }
        public int IDPrioridad { get; set; }
        public string PrioridadNombre { get; set; }
        public int IDCaracteristica { get; set; }
        public string CaracteristicaNombre { get; set; }
        public bool Resultado { get; set; }
        public string Actor { get; set; }
        public string Demandado { get; set; }
        public string IDUsuarioIngreso { get; set; }
        public string UsuarioComprueba { get; set; }
        public DateTime? FechaHoraRecibido { get; set; }
        public string IDUsuarioRecibido { get; set; }
        public int IDConfirmacion { get; set; }
        public string Observaciones { get; set; }
        public string NombreArchivo { get; set; }
        public string ConsecutivoCompleto { get; set; }
    }
}
