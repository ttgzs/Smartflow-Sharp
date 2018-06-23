/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Smartflow.Dapper;

namespace Smartflow
{
    public class WorkflowConfig : Infrastructure, IWorkflowConfiguration
    {
        public  DataTable GetWorkflowConfigs()
        {
            string query = " SELECT * FROM T_CONFIG ";
            DataTable configData = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader dr = Connection.ExecuteReader(query))
            {
                configData.Load(dr);
            }
            return configData;
        }
    }
}
