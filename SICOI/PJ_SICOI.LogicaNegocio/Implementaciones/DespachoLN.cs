using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.AccesoDatos.Accesos;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class DespachoLN
    {
        public static List<DespachoModel> ListarDespachos()
        {
            try
            {
                List<DespachoModel> despachos = DespachoAD.ListaDespachos();
                return despachos;
            } 
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }
        }

        public static string InsertarDespacho(string CodigoDespacho, string NombreDespacho)
        {
            if(CodigoDespacho == null || NombreDespacho == null)
            {
                return "No puede ingresar valores vacíos.";
            }
            if(CodigoDespacho.Length > 4 || CodigoDespacho.Length < 1)
            {
                return "El código del despacho es inválido.";
            }
            try
            {
                /*
                * Comentario por imendezca: Se omite el código de circuito en esta función ya que el procedimiento almacenado 
                * ingresa el código del circuito por defecto, su parametro por defecto es Goicoechea, por el momento solo funciona
                * para Goiecoeceha.
                */
                string resultado = DespachoAD.AgregarDespacho(CodigoDespacho, NombreDespacho);
                return resultado;
            }
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }
        }
    }
}
