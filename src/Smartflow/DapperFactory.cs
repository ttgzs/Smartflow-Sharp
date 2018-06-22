/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Smartflow.Enums;
using System.Data.OracleClient;

namespace Smartflow
{
    public class DapperFactory
    {
        public static IDbConnection CreateWorkflowConnection()
        {
            SmartflowConfiguration config = ConfigurationManager.GetSection("smartflowConfiguration") as SmartflowConfiguration;

            Assert.CheckNull(config, "smartflowConfiguration");
            Assert.StringNull(config.ConnectionString, "ConnectionString");
            Assert.StringNull(config.DatabaseCategory, "DatabaseCategory");

            DatabaseCategory dbc;
            if (Enum.TryParse(config.DatabaseCategory, true, out dbc) || String.IsNullOrEmpty(config.ConnectionString))
            {
                return DapperFactory.CreateConnection(dbc, config.ConnectionString);
            }
            else
            {
                throw new WorkflowException(ResourceManage.GetString(ResourceManage.CONNECTION_CONFIG));
            }
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
                    //ms 提供
                    connection = DatabaseService.CreateInstance(new OracleConnection(connectionString));
                    break;
                case DatabaseCategory.MySQL:
                    //需要自已提供Dll
                    //connection = DatabaseService.CreateInstance(new SqlConnection(connectionString));
                    break;
            }
            return connection;
        }
    }
}
