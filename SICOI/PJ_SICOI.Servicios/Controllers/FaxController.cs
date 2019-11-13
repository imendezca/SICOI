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
                List<FaxModel> faxes = FaxLN.ConsultaFaxesPorConsecutivo(ConsecutivoCompleto);
                string json = JsonConvert.SerializeObject(faxes);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError(error);
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
                ErrorLN.InsertarError(error);
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
                ErrorLN.InsertarError(error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al insertar los faxes. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }

        [HttpPost]
        public async Task<string> SubirDocumento()
        {
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
                ErrorLN.InsertarError("[PruebaController, Subir] " + e.Message);
                return "Error al subir el archivo. " + e.Message;
            }

            return "Archivo subido correctamente";
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
        public HttpResponseMessage DescargarDocumento(string NombreArchivo)
        {
            if (String.IsNullOrEmpty(NombreArchivo))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            string localFilePath;

            localFilePath = Path.Combine(L_RutaDestinoArchivos, NombreArchivo);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = NombreArchivo;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }
    }
}
