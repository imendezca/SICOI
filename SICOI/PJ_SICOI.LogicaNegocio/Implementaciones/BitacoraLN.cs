using PJ_SICOI.AccesoDatos.Accesos;
using PJ_SICOI.Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class BitacoraLN
    {
        public static string InsertarBitacoraNueva(BitacoraModel nuevaBitacora)
        {
            if (nuevaBitacora.IDUsuario == null || nuevaBitacora.Accion == null || nuevaBitacora.Descripcion == null ||
                nuevaBitacora.Pantalla == null)
            {
                throw new InvalidOperationException("No puede ingresar valores vacíos.");
            }
            try
            {
                string resultado = BitacoraAD.AgregarABitacora(nuevaBitacora);
                return resultado;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, BitacoraLN - InsertarBitacoraNueva]: " + error);
                return error;
            }
        }
    }
}
