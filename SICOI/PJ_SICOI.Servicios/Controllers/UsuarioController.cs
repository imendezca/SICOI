using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJ_SICOI.LogicaNegocio.Implementaciones;
using PJ_SICOI.Entidades.Modelos;
using Newtonsoft.Json;

namespace PJ_SICOI.Servicios.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage IniciarSesion(UsuarioModel P_Usuario)
        {
            try
            {
                var C_Resultado = new HttpResponseMessage(HttpStatusCode.OK);
                string V_JSON = UsuarioLN.IniciarSesion(P_Usuario);
                if(V_JSON.Substring(0,5) == "ERROR")
                {
                    throw new HttpRequestException(V_JSON.Substring(7));
                }
                C_Resultado.Content = new StringContent(V_JSON);
                C_Resultado.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return C_Resultado;
            }
            catch (Exception e)
            {
                string C_MensajeError = e.Message;
                var C_Resultado = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string V_Error = "Surgió un problema al iniciar sesión: " + C_MensajeError;
                string V_JSON = JsonConvert.SerializeObject(V_Error);
                C_Resultado.Content = new StringContent(V_JSON);
                return C_Resultado;
            }
        }
    }
}
