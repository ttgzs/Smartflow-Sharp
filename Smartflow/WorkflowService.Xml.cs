/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
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
            string sql = "SELECT * FROM T_FLOWXML WHERE WFID=@ID";

            return conn.Query<WorkflowXml>(sql, new { ID = flowID }).FirstOrDefault<WorkflowXml>();
        }
    }
}
