using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;

namespace PJ_SICOI.LogicaNegocio.Clases
{
    public class ActiveDirectoryLN
    {
        public static bool AutenticarUsuario(string V_NombreUsuario, string V_Contrasena)
        {
            bool V_Autenticado;
            using(PrincipalContext pc = new PrincipalContext(ContextType.Domain, "org.poder-judicial.go.cr"))
            {
                V_Autenticado = pc.ValidateCredentials(V_NombreUsuario, V_Contrasena, ContextOptions.Negotiate);
                var V_DatosUsuario = UserPrincipal.FindByIdentity(pc, V_NombreUsuario);
                var V_GruposUsuario = V_DatosUsuario.GetGroups().ToList();
            }
            return V_Autenticado;

        }
    }
}