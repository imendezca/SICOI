using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJ_SICOI.AccesoDatos
{
    public class ConnectionHelper
    {
        public static string StringConexion(string nombreBD)
        {
            string conn = ConfigurationManager.ConnectionStrings[nombreBD].ConnectionString;
            return conn;
        }
    }
}

