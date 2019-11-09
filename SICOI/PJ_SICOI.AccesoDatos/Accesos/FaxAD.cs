using PJ_SICOI.Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class FaxAD
    {
        public static string AgregarFax(FaxModel NuevoFax)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                try
                {
                    var V_Parametros = new DynamicParameters();

                    V_Parametros.Add("@P_CodDespacho", NuevoFax.CodDespacho);
                    V_Parametros.Add("@P_Asunto", NuevoFax.Asunto);
                    V_Parametros.Add("@P_Expediente", NuevoFax.Expediente);
                    V_Parametros.Add("@P_Tipo", NuevoFax.Tipo);
                    V_Parametros.Add("@P_CantFolios", NuevoFax.CantFolios);
                    V_Parametros.Add("@P_IDPrioridad", NuevoFax.IDPrioridad);
                    V_Parametros.Add("@P_IDCaracteristica", NuevoFax.IDPrioridad);
                    V_Parametros.Add("@P_Resultado", NuevoFax.Resultado);
                    V_Parametros.Add("@P_Actor", NuevoFax.Actor);
                    V_Parametros.Add("@P_Demandado", NuevoFax.Demandado);
                    V_Parametros.Add("@P_IDUsuarioIngreso", NuevoFax.IDUsuarioIngreso);
                    V_Parametros.Add("@P_Observaciones", NuevoFax.Observaciones);
                    V_Parametros.Add("@P_ResultadoNuevoFax", "", dbType: DbType.String, direction: ParameterDirection.Output);

                    var result = connection.Execute("dbo.PA_FAX_InsertarFaxNuevo", V_Parametros, commandType: CommandType.StoredProcedure);

                    string ConsecutivoNuevo = V_Parametros.Get<string>("@P_ResultadoNuevoFax");

                    return ConsecutivoNuevo;
                }
                catch (Exception e)
                {
                    string descripcionError = e.Message;
                    return "La inserción del Fax dió el siguiente error: " + descripcionError;
                }

            }
        }
    }
}
