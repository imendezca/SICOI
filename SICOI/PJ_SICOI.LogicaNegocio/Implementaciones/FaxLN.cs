using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.AccesoDatos.Accesos;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class FaxLN
    {
        public static string InsertarFaxNuevo(FaxModel nuevoFax)
        {
            if (nuevoFax.Asunto == null || nuevoFax.CodDespacho == null || nuevoFax.Expediente == null ||
                nuevoFax.Tipo == null || nuevoFax.CantFolios == 0 || nuevoFax.IDPrioridad == 0 ||
                nuevoFax.IDCaracteristica == 0 || nuevoFax.Actor == null || nuevoFax.Demandado == null ||
                nuevoFax.IDUsuarioIngreso == null)
            {
                throw new InvalidOperationException("No puede ingresar valores vacíos.");
            }
            try
            {
                string resultado = FaxAD.AgregarFax(nuevoFax);
                return resultado;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, FaxLN - InsertarFaxNuevo]: " + error);
                return error;
            }
            
        }
    }
}
