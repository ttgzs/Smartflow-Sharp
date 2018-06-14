/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
*/
using System;
using System.Collections.Generic;
using System.Linq;

using Dapper;
using Smartflow;

namespace Smartflow.BussinessService.WorkflowService
{
    public class WorkflowStructureService
    {
        
        public static void Delete(string IDENTIFICATION)
        {
            string sql = " DELETE FROM T_STRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION ";
            SqlHelper.CreateConnection().Execute(sql, new { IDENTIFICATION = IDENTIFICATION });
        }

        public static List<WorkflowStructure> GetWorkflowStructureList()
        {
            string sql = " SELECT * FROM T_STRUCTURE ";
            return SqlHelper.CreateConnection().Query<WorkflowStructure>(sql).ToList();
        }

        public static WorkflowStructure GetWorkflowStructure(string instanceID)
        {
            string sql = "SELECT * FROM T_STRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION";
            return SqlHelper.CreateConnection().Query<WorkflowStructure>(sql, new { IDENTIFICATION = instanceID })
                .FirstOrDefault<WorkflowStructure>();
        }
    }
}
