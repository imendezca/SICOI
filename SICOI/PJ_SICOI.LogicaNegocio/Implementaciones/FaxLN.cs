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
        public static FaxModel ConsultaFaxPorConsecutivo(string ConsecutivoCompleto)
        {
            try
            {
                FaxModel fax = FaxAD.ConsultaFaxPorConsecutivo(ConsecutivoCompleto);
                return fax;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, FaxLN - ConsultaFaxPorConsecutivo]: " + error);
                return null;
            }
        }
        public static List<FaxModel> ListarFaxes(string CodigoDespacho)
        {
            try
            {
                List<FaxModel> faxes = FaxAD.ConsultaFaxes(CodigoDespacho);
                return faxes;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, FaxLN - ListarFaxes(por despacho)]: " + error);
                return null;
            }
        }
        public static List<FaxModel> ListarFaxes(Fax_FiltroModel FiltroFax)
        {
            try
            {
                List<FaxModel> faxes = FaxAD.ConsultaFaxes(FiltroFax);
                return faxes;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, FaxLN - ListarFaxes(por filtro)]: " + error);
                return null;
            }
        }
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

                BitacoraModel V_NuevaBitacora = new BitacoraModel();
                V_NuevaBitacora.Accion = "Insertar";
                V_NuevaBitacora.Descripcion = "Insertó el nuevo fax: " + resultado;
                V_NuevaBitacora.IDUsuario = nuevoFax.IDUsuarioIngreso;
                V_NuevaBitacora.Pantalla = "Ingresar nuevo fax";
                BitacoraLN.InsertarBitacoraNueva(V_NuevaBitacora);

                return resultado;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, FaxLN - InsertarFaxNuevo]: " + error);
                return error;
            }
        }
        public static bool EnviarFaxAlDespacho(string ConsecutivoCompletoFax)
        {
            if (ConsecutivoCompletoFax == null)
            {
                throw new InvalidOperationException("No puede ingresar valores vacíos.");
            }
            string resultado = FaxAD.EnviarFaxADespacho(ConsecutivoCompletoFax);
            if(resultado == "1")
            {
                return true;
            }
            return false;
        }
        public static bool RecibirFax(string ConsecutivoFaxCompleto, string IDUsuarioRecibe)
        {
            if (ConsecutivoFaxCompleto == null || IDUsuarioRecibe == null)
            {
                throw new InvalidOperationException("No puede ingresar valores vacíos.");
            }
            if (!UsuarioLN.UsuarioPerteneceACorreoInterno(IDUsuarioRecibe))
            {
                string resultado = FaxAD.RecibirFax(ConsecutivoFaxCompleto, IDUsuarioRecibe);
                if (resultado == "1")
                {
                    return true;
                }
            } 
            else
            {
                return true;
            }
            return false;
        }
    }
}
