using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Infrastructure;

namespace Smartflow.BussinessService
{
    public class WorkflowXmlService
    {
        public static void Delete(string WFID)
        {
            string sql = " DELETE FROM T_FLOWXML WHERE WFID=@WFID ";

            SqlHelper.CreateConnection().Execute(sql, new { WFID = WFID });
        }

        public static List<WorkflowXml> GetWorkflowXmlList()
        {
            string sql = " SELECT * FROM T_FLOWXML ";
            return SqlHelper.CreateConnection().Query<WorkflowXml>(sql).ToList();
        }


        public static WorkflowXml GetWorkflowXml(string instanceID)
        {
            string sql = "SELECT * FROM T_FLOWXML WHERE WFID=@WFID";

            return SqlHelper.CreateConnection().Query<WorkflowXml>(sql, new { WFID = instanceID })
                .FirstOrDefault<WorkflowXml>();
        }
    }
}
