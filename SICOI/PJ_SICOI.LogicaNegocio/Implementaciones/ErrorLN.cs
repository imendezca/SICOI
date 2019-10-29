using PJ_SICOI.AccesoDatos.Accesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class ErrorLN
    {
        public static string InsertarError(string P_DetalleError)
        {
            if (P_DetalleError == null )
            {
                return "No puede ingresar errores vacíos.";
            }
            try
            {
                string C_Resultado = ErrorAD.AgregarError(P_DetalleError);
                return C_Resultado;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorAD.AgregarError("[LogicaNegocio, ErrorLN-InsertarError]: " + error);
                return error;
            }
        }
    }
}
