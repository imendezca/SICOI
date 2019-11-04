using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.LogicaNegocio.Implementaciones;

namespace PJ_SICOI.Servicios.Controllers
{
    public class CaracteristicaController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ConsultaCaracteristicas()
        {
            try
            {
                var V_Result = new HttpResponseMessage(HttpStatusCode.OK);
                List<CaracteristicaModel> V_Caracteristicas = CaracteristicaLN.ListarCaracteristicas();
                string json = JsonConvert.SerializeObject(V_Caracteristicas);
                V_Result.Content = new StringContent(json);
                V_Result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return V_Result;
            }
            catch (Exception e)
            {
                string V_Error = e.Message;
                var V_Result = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string V_V_Resultado = "Surgió un problema al obtener los datos" + V_Error;
                V_Result.Content = new StringContent(V_V_Resultado);
                return V_Result;
            }
        }
    }
}
