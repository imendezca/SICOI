using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJ_SICOI.Entidades.Modelos;
using PJ_SICOI.AccesoDatos.Accesos;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PJ_SICOI.LogicaNegocio.Implementaciones
{
    public class UsuarioLN
    {
        public static string IniciarSesion(UsuarioModel P_Usuario)
        {
            try
            {
                string V_Respuesta;
                bool C_InicioSesion = UsuarioAD.IniciarSesionUsuario(P_Usuario);

                if (!C_InicioSesion)
                {
                    V_Respuesta = "ERROR: El usuario o la contraseña no son correctos. Intentelo de nuevo";
                    throw new Exception(V_Respuesta);
                }

                UsuarioModel usuario = UsuarioAD.ConsultaUsuario(P_Usuario.IDUsuario);
                usuario.RolesUsuario = UsuarioAD.ConsultaRoles(P_Usuario.IDUsuario);

                if (usuario.RolesUsuario.Count == 0)
                {
                    V_Respuesta = "ERROR: El usuario " + P_Usuario.IDUsuario + " no tiene ningún rol asignado.";
                    throw new Exception(V_Respuesta);
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aM4U2YjoHBiRYi8zVcVQSgXu7NbQuVrW"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var claims = new List<Claim>();

                claims.Add(new Claim("IDUsuario", usuario.IDUsuario));
                claims.Add(new Claim("CodDespacho", usuario.CodDespacho));
                foreach (UsuarioModel.Rol Rol in usuario.RolesUsuario)
                {
                    claims.Add(new Claim("RolesUsuario", Rol.IDRol));
                }

                var token = new JwtSecurityToken(
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: creds,
                        claims: claims
                    );
                var TokenString = new JwtSecurityTokenHandler().WriteToken(token);

                usuario.TokenUsuario = TokenString;

                V_Respuesta = JsonConvert.SerializeObject(usuario);
                return V_Respuesta;
            }
            catch (Exception e)
            {
                string C_Error = e.Message;
                ErrorLN.InsertarError("[LogicaNegocio, UsuarioLN - ConsultaUsuario]: " + C_Error);
                return C_Error;
            }
        }

        public static UsuarioModel ObtenerDatosUsuario(string P_Usuario)
        {
            /*
             * Función para obtener datos del usuario.
             * Para uso único interno del API
             */
            UsuarioModel usuario = UsuarioAD.ConsultaUsuario(P_Usuario);
            usuario.RolesUsuario = UsuarioAD.ConsultaRoles(P_Usuario);
            return usuario;
        }

        public static bool UsuarioPerteneceACorreoInterno(string P_Usuario)
        {
            UsuarioModel Usuario = UsuarioLN.ObtenerDatosUsuario(P_Usuario);
            if (Usuario.CodDespacho == "0176")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
