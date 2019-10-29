using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJ_SICOI.AccesoDatos.Accesos;
using PJ_SICOI.Entidades.Modelos;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class CaracteristicaLN
    {
        public static List<CaracteristicaModel> ListarCaracteristicas()
        {
            try
            {
                List<CaracteristicaModel> caracteristica = CaracteristicaAD.ListaCaracteristicas();
                return caracteristica;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio,  CaracteristicaLN - ListarCaracteristicas]: " + error);
                return null;
            }
        }
    }
}
