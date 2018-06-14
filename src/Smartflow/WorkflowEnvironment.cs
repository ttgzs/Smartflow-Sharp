using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Smartflow
{
    public class WorkflowEnvironment
    {
        public static DataTable GetWorkflowConfigs()
        {
            string query = " SELECT * FROM T_CONFIG ";
            DataTable configData = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader dr = DapperFactory.CreateWorkflowConnection().ExecuteReader(query))
            {
                configData.Load(dr);
            }
            return configData;
        }
    }
}
