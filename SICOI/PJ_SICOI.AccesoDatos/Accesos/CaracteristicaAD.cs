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
    public class CaracteristicaAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de CARACTERISTICA.
        /// </summary>
        public static List<CaracteristicaModel> ListaCaracteristicas()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.Query<CaracteristicaModel>("dbo.PA_CARACTERISTICA_ListarCaracteristicas").ToList();
            }
        }
    }
}
