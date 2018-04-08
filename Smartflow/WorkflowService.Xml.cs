using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Dapper;

namespace Smartflow
{
    public partial class WorkflowService
    {
        public WorkflowXml GetWorkflowXml(string flowID)
        {
            string sql = "SELECT * FROM T_FLOWXML WHERE ID=@ID";

            return conn.Query<WorkflowXml>(sql, new { ID = flowID }).FirstOrDefault<WorkflowXml>();
        }
    }
}
