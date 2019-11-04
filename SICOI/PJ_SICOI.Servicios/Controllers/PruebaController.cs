using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace PJ_SICOI.Servicios.Controllers
{
    public class PruebaController : ApiController
    {
        [HttpPost]
        public async Task<string> Subir()
        {
            var provider = new MultipartFormDataStreamProvider("\\\\10.11.4.13\\CompartidaPrueba");

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach(var file in provider.FileData)
                {
                    var name = file.Headers.ContentDisposition.FileName;

                    name = name.Trim('"');

                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine("\\\\10.11.4.13\\CompartidaPrueba", name);

                    File.Move(localFileName, filePath);
                }
            } 
            catch (Exception e)
            {
                return "Error";
            }

            return "subido";
        }
    }
}
