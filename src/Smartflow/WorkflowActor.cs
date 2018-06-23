/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using Smartflow.Dapper;
using Smartflow.Elements;

namespace Smartflow
{
    public class WorkflowActor : Actor, IWorkflowActor
    {
        public DataTable GetRecord(string instanceID)
        {
            string sql = ResourceManage.GetString(ResourceManage.SQL_ACTOR_RECORD);

            LogService.Info(string.Format("查询获取参与者 SQL：{0} INSTANCEID:{1}", sql, instanceID));

            using (IDataReader dr = Connection.ExecuteReader(sql, new { INSTANCEID = instanceID }))
            {
                DataTable dt = new DataTable(Guid.NewGuid().ToString());
                dt.Load(dr);
                return dt;
            }
        }
    }
}
