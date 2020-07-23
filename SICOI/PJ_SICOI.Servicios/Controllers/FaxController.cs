using Newtonsoft.Json;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.LogicaNegocio.Implementaciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace PJ_SICOI.Servicios.Controllers
{
    public class FaxController : ApiController
    {
        string L_RutaDestinoArchivos = "E:\\Otra\\Archivos";

        [HttpGet]
        public HttpResponseMessage ConsultaFaxPorConsecutivo(string ConsecutivoCompleto)
        {
            var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                FaxModel fax = FaxLN.ConsultaFaxPorConsecutivo(ConsecutivoCompleto);
                string json = JsonConvert.SerializeObject(fax);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[FaxController, ConsultaFaxPorConsecutivo] " + error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al consultar los faxes. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }

        [HttpGet]
        public HttpResponseMessage ConsultaFaxPorDespacho(string CodDespacho)
        {
            var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                List<FaxModel> faxes = FaxLN.ListarFaxes(CodDespacho);
                string json = JsonConvert.SerializeObject(faxes);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[FaxController, ConsultaFaxPorDespacho] " + error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al consultar los faxes. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }

        [HttpPost]
        public HttpResponseMessage ConsultaFaxPorFiltro(Fax_FiltroModel FiltroFax)
        {
            var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                List<FaxModel> faxes = FaxLN.ListarFaxes(FiltroFax);
                string json = JsonConvert.SerializeObject(faxes);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[FaxController, ConsultaFaxPorFiltro] " + error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al consultar los faxes. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }

        [HttpPost]
        public HttpResponseMessage NuevoFax(FaxModel fax)
        {
            var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                string resultado = FaxLN.InsertarFaxNuevo(fax);
                string json = JsonConvert.SerializeObject(resultado);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[FaxController, NuevoFax] " +  error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al insertar los faxes. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }

        [HttpPost]
        public HttpResponseMessage EnviarAlDespacho(string ConsecutivoCompletoFax)
        {
            var V_RespuestaWS = new HttpResponseMessage(HttpStatusCode.OK);
            string V_RespuestaTexto;
            try
            {
                bool V_Resultado = FaxLN.EnviarFaxAlDespacho(ConsecutivoCompletoFax);
                if (V_Resultado)
                {
                    V_RespuestaTexto = "El estado del fax cambió correctamente.";
                }
                else
                {
                    throw new HttpRequestException("No se pudo cambiar el estado del fax");
                }
                string V_Json = JsonConvert.SerializeObject(V_RespuestaTexto);
                V_RespuestaWS.Content = new StringContent(V_Json);
                V_RespuestaWS.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return V_RespuestaWS;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[FaxController, EnviarAlDespacho] " + error);
                V_RespuestaWS = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject(error);
                V_RespuestaWS.Content = new StringContent(json);
                return V_RespuestaWS;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubirDocumento()
        {
            var V_RespuestaWS = new HttpResponseMessage(HttpStatusCode.OK);
            V_RespuestaWS.Content = new StringContent(String.Empty);
            V_RespuestaWS.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            string V_Json;

            string rutaArchivosTemporales = "E:\\Otra\\Temp";
            var provider = new MultipartFormDataStreamProvider(rutaArchivosTemporales);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                //var path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                foreach (var archivo in provider.FileData)
                {
                    if (archivo.Headers.ContentType.MediaType != "application/pdf")
                    {
                        throw new InvalidOperationException("Uno de los archivos no es de formato PDF");
                    }
                    var name = archivo.Headers.ContentDisposition.Name;
                    name = name.Trim('"');

                    var localFileName = archivo.LocalFileName;
                    string filePath = Path.Combine(L_RutaDestinoArchivos, name);
                    filePath = VerificaExiste(filePath, name, L_RutaDestinoArchivos);
                    /*
                     * Activar en caso de que se necesite otro usuario para guardar archivos
                     * using (new Impersonator("[USUARIO]", "[DOMINIO]PODER-JUDICIAL", "[CONTRASÑA]"))
                    {
                        string user = WindowsIdentity.GetCurrent().Name;
                        File.Move(localFileName, filePath);
                        File.Delete(localFileName);
                    }*/
                    File.Move(localFileName, filePath);
                    File.Delete(localFileName);

                }
            }
            catch (Exception e)
            {
                ErrorLN.InsertarError("[FaxController, SubirDocumento] " + e.Message);

                V_RespuestaWS = new HttpResponseMessage(HttpStatusCode.BadRequest);
                V_Json = JsonConvert.SerializeObject("Error al subir el archivo. " + e.Message);
                V_RespuestaWS.Content = new StringContent(V_Json);
                return V_RespuestaWS;
            }

            V_Json = JsonConvert.SerializeObject("Archivo subido correctamente");
            V_RespuestaWS.Content = new StringContent(V_Json);
            return V_RespuestaWS;
        }

        private string VerificaExiste(string RutaArchivo, string NombreArchivo, string RutaDestino)
        {
            /// <summary>
            /// Cambia el nombre del archivo, siempre y cuando este ya exista en el sistema de archivos
            /// </summary>
            if (File.Exists(RutaArchivo))
            {
                NombreArchivo = NombreArchivo.Replace(".pdf", "");
                NombreArchivo += " (1)";
                NombreArchivo += ".pdf";
                RutaArchivo = Path.Combine(RutaDestino, NombreArchivo);
                return VerificaExiste(RutaArchivo, NombreArchivo, RutaDestino);
            }
            else
            {
                return RutaArchivo;
            }
        }

        [HttpGet]
        public HttpResponseMessage DescargarDocumento(string ConsecutivoFaxCompleto, string IDUsuarioRecibe)
        {
            if (String.IsNullOrEmpty(ConsecutivoFaxCompleto))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            FaxModel FaxConsultado = FaxLN.ConsultaFaxPorConsecutivo(ConsecutivoFaxCompleto);

            string NombreArchivoADescargar = FaxConsultado.NombreArchivo;

            string V_RutaLocalArchivos;
            V_RutaLocalArchivos = Path.Combine(L_RutaDestinoArchivos, NombreArchivoADescargar);

            HttpResponseMessage respuesta = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                respuesta.Content = new StreamContent(new FileStream(V_RutaLocalArchivos, FileMode.Open, FileAccess.Read));
                respuesta.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                respuesta.Content.Headers.ContentDisposition.FileName = NombreArchivoADescargar;
                respuesta.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                FaxLN.RecibirFax(ConsecutivoFaxCompleto, IDUsuarioRecibe);
            }
            catch(FileNotFoundException e)
            {
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                ErrorLN.InsertarError("[FaxController, DescargarDocumento] " + e.Message);
            }
            
            return respuesta;
        }
    }
}
