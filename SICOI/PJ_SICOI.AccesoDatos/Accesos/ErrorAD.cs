using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class ErrorAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de despacho.
        /// </summary>

        public static string AgregarError(string P_DetalleError)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                try
                {
                    var result = connection.Execute("dbo.PA_ERROR_InsertarError @P_DetalleError", new
                    {
                        P_DetalleError = P_DetalleError
                    });
                    return "El error fue registrado con éxito.";
                }
                catch (Exception e)
                {
                    string descripcionError = e.Message;
                    return "La inserción del error dió el siguiente error: " + descripcionError;
                }

            }
        }
    }
}
