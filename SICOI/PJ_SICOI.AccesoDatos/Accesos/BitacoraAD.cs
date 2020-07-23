using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using PJ_SICOI.Entidades.Modelos;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class BitacoraAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de Bitacora.
        /// </summary>

        public static string AgregarABitacora(BitacoraModel Bitacora)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                try
                {
                    var V_Parametros = new DynamicParameters();

                    V_Parametros.Add("@P_IDUsuario", Bitacora.IDUsuario);
                    V_Parametros.Add("@P_Accion", Bitacora.Accion);
                    V_Parametros.Add("@P_Descripcion", Bitacora.Descripcion);
                    V_Parametros.Add("@P_Pantalla", Bitacora.Pantalla);

                    var result = connection.Execute("dbo.PA_BITACORA_InsertaBitacora", V_Parametros, commandType: CommandType.StoredProcedure);

                    return "El registro a bitácora fue registrado con éxito.";
                }
                catch (Exception e)
                {
                    string descripcionError = e.Message;
                    return "La inserción a la bitácora dió el siguiente error: " + descripcionError;
                }

            }
        }
    }
}
