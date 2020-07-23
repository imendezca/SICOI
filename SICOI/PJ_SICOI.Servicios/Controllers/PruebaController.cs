using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using PJ_SICOI.LogicaNegocio.Implementaciones;
using System.Security.Principal;
using PJ_SICOI.Servicios.Utilitarios;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Web.Configuration;
using System.Net.Mail;
using PJ_SICOI.LogicaNegocio.Clases;

namespace PJ_SICOI.Servicios.Controllers
{
    public class PruebaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Token()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["ClaveSecreta"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Role, "Primero"));
            claims.Add(new Claim(ClaimTypes.Role, "Segundo"));
            claims.Add(new Claim("variable", "valorVariable"));

            var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddSeconds(20),
                    signingCredentials: creds,
                    claims: claims
                );
            var TokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(TokenString);
        }

        [HttpGet]
        public IHttpActionResult Leer(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenDesc = handler.ReadJwtToken(token);
            return Ok(tokenDesc);
        }

        [HttpGet]
        public IHttpActionResult Enviar()
        {
            string from = "goi-print@poder-judicial.go.cr";
            string to = "imendezca@poder-judicial.go.cr";
            string subject = "Pruebas";
            string message = @"<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""><label value=""case_identity_713057_8"" visible=""false""> <html>
    <head>
        
        <title>Notificacion Correo</title>
    </head>
    <body style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; margin: 0; padding: 0; width: 100%; -webkit-text-size-adjust: none; -webkit-font-smoothing: antialiased;"" class=""RadEContent"" marginwidth=""0"" marginheight=""0"">
        <style type=""text/css"">
            @media only screen and (max-width: 480px) {
            table {
            display: block !important;
            width: 100% !important;
            }
            td {
            width: 480px !important;
            }
            }
        </style>
        <table id=""background"" style=""height: 100% !important; margin: 0; padding: 0; width: 100% !important; clear: left;"" width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#FFFFFF"">
            <tbody>
                <tr>
                    <td valign=""top"" align=""center"">
                    <table id=""preheader"" width=""600"" cellspacing=""0"" cellpadding=""20"" border=""0"" bgcolor=""#FFFFFF"">
                        <tbody>
                            <tr>
                                <td valign=""top"" height=""100"">
                                <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                    <tbody>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td style=""text-align:center"" valign=""top""><img src=""https://sjoaranda.poder-judicial.go.cr/afs/images/logo.png"" alt=""""></td>
                                        </tr>
                                        <tr>
                                            <td valign=""top"" height=""5""><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width=""600"" valign=""top"">
                                            <div class=""logo"">
                                            <a href=""javascript:void(0)"" onclick=""myEvent();"" onmouseover=""this.style.color='#000000'"" onmouseout=""this.style.color='#514F4E'"" style=""color: #514F4E; font-size: 18px; font-weight: bold; text-align: left; text-decoration: none;""></a>
                                            </div>
                                            <br>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- // END #preheader -->
                    <table id=""header_container"" width=""600"" cellspacing=""0"" cellpadding=""0"" border=""0"" bgcolor=""#FFFFFF"">
                        <tbody>
                            <tr>
                                <td valign=""top"" align=""center"">
                                <!-- // END #header -->
                                <br>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table id=""body_container"" width=""600"" cellspacing=""0"" cellpadding=""10"" border=""0"" bgcolor=""#0B5C8C"">
                        <tbody>
                            <tr>
                                <td height=""20"" align=""center"">
                                <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                    <tbody>
                                        <tr>
                                            <td height=""20"">
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #ffffff; font-size: 20px; text-align: center; margin: 0px;""><strong>Se le ha asignado un INCIDENTE</strong></p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table id=""body_container"" width=""600"" cellspacing=""0"" cellpadding=""20"" border=""0"" bgcolor=""#FFFFFF"">
                        <tbody>
                            <tr>
                                <td class=""body_content"" style=""padding-bottom: 0px; padding-top: 0px;"" height=""100"" align=""center"">
                                <table width=""100%"" cellspacing=""0"" cellpadding=""20"" border=""0"">
                                    <tbody>
                                        <tr>
                                            <td valign=""top"">
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 12px; text-align: center;""><strong>A continuación se presentará el resumen del reporte</strong>.</p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left"">Número de reporte: <strong>INCIDENTE #203846</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Asunto: <strong>Problemas con el mouse</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Fecha y hora del reporte: <strong>Feb  6 2020  8:12AM</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Categoría: <strong>Problema con equipo</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Servicio: <strong>1. Soporte a usuarios</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Persona a contactar: <strong>Dayana Londoño Corea</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Teléfono: <strong>01-92-48</strong></p>
                                            <p style=""font-fam ily: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Ubicación: <strong>JUZGADO PENAL DEL II CIRCUITO JUDICIAL DE SAN JOSE - 515</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Correo electrónico: <strong>dlondonoc@Poder-Judicial.go.cr</strong></p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;"">Detalle:</p>
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;""><strong>Ubicada 6 piso oficina 614 problemas con mouse es inalambrico no funciona y ya le cambio baterías, también se movió de puertos&nbsp;</strong></p>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- // END #body_container -->
                    <table id=""body_info_container"" width=""600"" cellspacing=""0"" cellpadding=""20"" border=""0"" bgcolor=""#ffffff"">
                        <tbody>
                            <tr>
                                <td class=""body_info_content"" style=""padding-bottom: 0px; padding-top: 0px;"" valign=""top"" height=""84"" align=""center"">
                                <table width=""100%"" cellspacing=""0"" cellpadding=""20"" border=""0"">
                                    <tbody>
                                        <tr>
                                            <td valign=""top"" height=""50"">
                                            <hr style=""background-color: #000000; border: none; color: #000000; height: 2px; margin-top: 0px;"">
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;""><a href=""https://sjoaranda.poder-judicial.go.cr/DTIC/Especialistas"">Haga clic aquí para ver más información relacionada con su reporte</a></p>
                                            <hr style=""background-color: #000000; border: none; color: #000000; height: 2px; margin-top: 0px;"">
                                            <p style=""font-family: 'Verdana','Arial','Helvetica Neue','Helvetica','sans-serif'; color: #000000; font-size: 14px; line-height: 22px; text-align: left;""><strong><span style=""text-decoration: underline;"">IMPORTANTE:</span></strong> Este correo es informativo, no responda a esta dirección de correo, ya que no está habilitada para recibir mensajes. <br>
                                            Si requiere más información sobre el contenido de este mensaje, comuníquese a la extensión 014777 con los Oficiales de Mesa de Servicio.</p>
                                            <hr style=""background-color: #000000; border: none; color: #000000; height: 2px; margin-top: 0px;"">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <!-- // END #contact -->
                    </td>
                </tr>
            </tbody>
        </table>
        <!-- // END #contact_container --><!-- // END #footer_container -->
        <!-- // END #background --> <br>
    </body>
</html>
";


            MailMessage mail = new MailMessage(from, to);
            mail.Subject = "Prueba";
            string mensaje = "<h1 style=\"color: white;\">Prueba de html</h1>";
            mail.Body = message;
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("correointerno", 25);
            client.Send(mail);
            return Ok();
        }


        [HttpGet]
        public IHttpActionResult loguear(string use, string con)
        {
            ActiveDirectoryLN.AutenticarUsuario(use, con);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult encriptar(string con)
        {
            byte[] bitsitos = Encoding.ASCII.GetBytes("PruebasBonitas");
            string  resultado = Encoding.ASCII.GetString(bitsitos);
            con = Encriptacion.EncryptStringToBytes_Aes(con);
            return Ok(con);
        }

    }
}
