using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJ_SICOI.Entidades.Modelos;
using Dapper;
using System.Data;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class PrioridadAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de prioridad.
        /// </summary>
        public static List<PrioridadModel> ListaPrioridades()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.Query<PrioridadModel>("dbo.PA_PRIORIDAD_ListarPrioridades").ToList();
            }
        }
    }
}
