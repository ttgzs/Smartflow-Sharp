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
            string sql = " SELECT Y.IDENTIFICATION,X.APPELLATION,X.CREATEDATETIME,X.OPERATION FROM T_ACTOR X,T_NODE Y WHERE X.INSTANCEID=@INSTANCEID AND X.RNID=Y.NID ORDER BY CREATEDATETIME ASC ";
            using (IDataReader dr = DapperFactory.CreateWorkflowConnection().ExecuteReader(sql, new { INSTANCEID = instanceID }))
            {
                DataTable dt = new DataTable(Guid.NewGuid().ToString());
                dt.Load(dr);
                return dt;
            }
        }
    }
}
