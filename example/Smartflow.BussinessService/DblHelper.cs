using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Smartflow.BussinessService
{
    public class DblHelper
    {
        public static IDbConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["busConnection"];
            IDbConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
