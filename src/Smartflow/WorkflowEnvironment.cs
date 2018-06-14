using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Smartflow.Dapper;

namespace Smartflow
{
    public class WorkflowEnvironment
    {
        public const string CONST_SMARTFLOW_SHARP_NAME = "Smartflow-Sharp";
        public const string CONST_SMARTFLOW_SHARP_TITLE = "工作流管理平台";
        public const string CONST_SMARTFLOW_SHARP_VERSION = "1.0";
        
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
