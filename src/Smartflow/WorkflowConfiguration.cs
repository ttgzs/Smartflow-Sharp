using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Smartflow.Dapper;

namespace Smartflow
{
    public class WorkflowConfiguration : IWorkflowConfiguration
    {
        public  DataTable GetWorkflowConfigs()
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
