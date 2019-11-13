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
    public class DespachoAD
    {
        /// <summary>
        /// Clase donde se almacenan las funciones necesarias para la conexión con la base de datos
        /// con lo que respecta a la entidad de despacho.
        /// </summary>
        public static List<DespachoModel> ConsultaDespachos()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                return connection.Query<DespachoModel>("dbo.PA_DESPACHO_ListarDespachos").ToList();
            }
        }

        public static string AgregarDespacho(string CodigoDespacho, string Nombre)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionHelper.StringConexion("SICOI_DB")))
            {
                try
                {
                    /*
                     * Comentario por imendezca: Se omite el código de circuito en este procedimiento almacenado, ya que su parametro
                     * por defecto es Goicoechea, por el momento solo funciona para Goiecoeceha.
                     */
                    var result = connection.Execute("dbo.PA_DESPACHO_InsertarNuevo @P_CodDespacho, @P_Nombre", new { P_CodDespacho = CodigoDespacho,
                                                                                                                     P_Nombre = Nombre });
                    return "El despacho fue agregado con éxito.";
                }
                catch (Exception e)
                {
                    string descripcionError = e.Message;
                    return "La inserción del despacho dió el siguiente error: " + descripcionError;
                }

            }
        }
    }
}
