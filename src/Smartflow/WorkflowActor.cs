using System;
using System.Collections.Generic;
using System.Data;
using Smartflow.Dapper;

namespace Smartflow
{
    public class WorkflowActor
    {
        public static DataTable GetRecord(string instanceID)
        {
            string sql=ResourceManage.GetString(ResourceManage.SQL_ACTOR_RECORD);
            using (IDataReader dr = DapperFactory.CreateWorkflowConnection().ExecuteReader(sql, new { INSTANCEID = instanceID }))
            {
                DataTable dt = new DataTable(Guid.NewGuid().ToString());
                dt.Load(dr);
                return dt;
            }
        }
    }
}
