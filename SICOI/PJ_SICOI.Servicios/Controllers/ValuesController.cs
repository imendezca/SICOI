using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PJ_SICOI.AccesoDatos.Accesos;
using PJ_SICOI.Entidades.Entidades;

namespace PJ_SICOI.Servicios.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public HttpResponseMessage Intento()
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                List<DespachoModel> despachos = Despacho.ListaDespachos();
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
        public HttpResponseMessage Insertar()
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                string resultado = Despacho.AgregarDespacho("0003", "Algo divi");
                string json = JsonConvert.SerializeObject(resultado);
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

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
