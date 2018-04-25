/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Smartflow.Web.Code
{
    public class DapperHelper
    {
        public static IDbConnection CreateWorkflowConnection()
        {
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["busConnection"];
            IDbConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
