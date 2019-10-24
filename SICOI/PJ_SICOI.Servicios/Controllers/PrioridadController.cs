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
    public class PrioridadController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ConsultaPrioridades()
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                List<PrioridadModel> prioridades = PrioridadLN.ListarPrioridades();
                string json = JsonConvert.SerializeObject(prioridades);
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
    }
}
