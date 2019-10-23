using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PJ_SICOI.Entidades.Entidades;

namespace PJ_SICOI.AccesoDatos.Accesos
{
    public class Despacho
    {
        public static List<DespachoModel> ListaDespachos()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.Query<DespachoModel>($"select * from DESPACHO").ToList();
            }
        }

        public static string AgregarDespacho(string CodigoDespacho, string Nombre)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                var result = connection.Execute("dbo.PA_DESPACHO_InsertarNuevo @P_CodDespacho, @P_Nombre", new { P_CodDespacho = CodigoDespacho, 
                                                                                                                                P_Nombre      = Nombre });
                return "todo bien";
            }
        }
    }
}
