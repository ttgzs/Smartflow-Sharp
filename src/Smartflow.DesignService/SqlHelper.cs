/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Smartflow.DesignService
{
    public class SqlHelper
    {
        public static IDbConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["busConnection"];
            IDbConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
