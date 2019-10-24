using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJ_SICOI.LogicaNegocio.Implementaciones;
using PJ_SICOI.Entidades.Modelos;
using Newtonsoft.Json;

namespace PJ_SICOI.Servicios.Controllers
{
    public class DespachoController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ConsultaDespachos()
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                List<DespachoModel> despachos = DespachoLN.ListarDespachos();
                string json = JsonConvert.SerializeObject(despachos);
                result.Content = new StringContent(json);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return result;
            }
            catch (Exception e)
            {
                string error = e.Message;
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                result.Content = new StringContent("Surgió un problema al obtener los datos");
                return result;
            }
        }

        [HttpPost]
        public HttpResponseMessage Insertar(DespachoModel despacho)
        {
            try
            {
                var respuesta = new HttpResponseMessage(HttpStatusCode.OK);
                string resultado = DespachoLN.InsertarDespacho(despacho.CodDespacho, despacho.Nombre);
                string json = JsonConvert.SerializeObject(resultado);
                respuesta.Content = new StringContent(json);
                respuesta.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return respuesta;
            }
            catch (Exception e)
            {
                string error = e.Message;
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string json = JsonConvert.SerializeObject("Surgió un problema al obtener los datos");
                result.Content = new StringContent(json);
                return result;
            }
        }
    }
}
