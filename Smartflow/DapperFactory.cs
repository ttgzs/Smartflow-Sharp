////////////////////////////////////////////////////////////////////////////
///
////////////////////////////////////////////////////////////////////////////
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Smartflow.Enums;


namespace Smartflow
{
    public class DapperFactory
    {
        public static IDbConnection CreateWorkflowConnection()
        {
            string flowConnection = ConfigurationManager.AppSettings["flowConnection"];
            return DapperFactory.CreateConnection(DatabaseCategory.SQLServer, flowConnection);
        }

        public static IDbConnection CreateConnection(DatabaseCategory dbc, string connectionString)
        {
            IDbConnection connection = null;
            switch (dbc)
            {
                case DatabaseCategory.SQLServer:
                    connection = DatabaseService.CreateInstance(new SqlConnection(connectionString));
                    break;
                case DatabaseCategory.Oracle:
                    connection = DatabaseService.CreateInstance(new SqlConnection(connectionString));
                    break;
                case DatabaseCategory.MySQL:
                    connection = DatabaseService.CreateInstance(new SqlConnection(connectionString));
                    break;
            }
            return connection;
        }
    }
}
