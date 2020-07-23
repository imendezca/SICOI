using Newtonsoft.Json;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.LogicaNegocio.Implementaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PJ_SICOI.Servicios.Controllers
{
    public class BitacoraController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage NuevaBitacora(BitacoraModel bitacora)
        {
            var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                string resultado = BitacoraLN.InsertarBitacoraNueva(bitacora);
                string json = JsonConvert.SerializeObject(resultado);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                ErrorLN.InsertarError("[BitacoraController, NuevaBitacora] " + error);
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al insertar los datos de bitácora. " + error);
                respuesta.Content = new StringContent(json);
                return respuesta;
            }
        }
    }
}
