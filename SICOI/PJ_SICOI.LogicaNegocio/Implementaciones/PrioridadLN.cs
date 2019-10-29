using PJ_SICOI.AccesoDatos.Accesos;
using PJ_SICOI.Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class PrioridadLN
    {
        public static List<PrioridadModel> ListarPrioridades()
        {
            try
            {
                List<PrioridadModel> prioridades = PrioridadAD.ListaPrioridades();
                return prioridades;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio,  PrioridadLN - ListarPrioridades]: " + error);
                return null;
            }
        }
    }
}
