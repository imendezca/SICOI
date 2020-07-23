using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PJ_SICOI.Entidades.Modelos;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class UsuarioAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de Usuario y los roles.
        /// </summary>

        public static UsuarioModel ConsultaUsuario(string NombreUsuario)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.QueryFirst<UsuarioModel>("dbo.PA_USUARIO_ConsultarUsuario @P_Usuario", new { P_Usuario = NombreUsuario });
            }
        }

        public static List<UsuarioModel.Rol> ConsultaRoles(string NombreUsuario)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.Query<UsuarioModel.Rol>("dbo.PA_USUARIO_ConsultaRolesActuales @P_Usuario", new { P_Usuario = NombreUsuario }).ToList();
            }
        }

        public static bool IniciarSesionUsuario(UsuarioModel UsuarioLogin)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                var V_Parametros = new DynamicParameters();
                V_Parametros.Add("@P_Usuario", UsuarioLogin.IDUsuario);
                V_Parametros.Add("@P_Contrasena", UsuarioLogin.Contrasena);
                V_Parametros.Add("@P_Resultado", "", dbType: DbType.String, direction: ParameterDirection.Output);

                var V_Resultt = connection.Execute("dbo.PA_USUARIO_IniciaSesion", V_Parametros, commandType: CommandType.StoredProcedure);

                bool InicioCorrecto = V_Parametros.Get<string>("@P_Resultado") == "1"? true: false;

                return InicioCorrecto;
            }
        }
    }
}
