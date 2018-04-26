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

using Dapper;
using System.Data;

namespace Smartflow.Web.Code
{
    public class WorkflowUtils
    {

       

        public static void Persistent(WorkflowXml model)
        {
            string sql = "INSERT INTO T_FLOWXML(WFID,NAME,XML,ORIGIN)  VALUES(@WFID,@NAME,@XML,@ORIGIN)";
            IDbConnection connection = DapperHelper.CreateWorkflowConnection();
            connection.Execute(sql, model);
        }

        public static void ModWorkflowXml(WorkflowXml model)
        {
            string sql = " UPDATE T_FLOWXML SET NAME=@NAME,XML=@XML,ORIGIN=@ORIGIN WHERE WFID=@WFID ";
            IDbConnection connection = DapperHelper.CreateWorkflowConnection();
            connection.Execute(sql, model);
        }

        public static WorkflowXml GetWorkflowXml(string flowID)
        {
            string sql = " SELECT * FROM  T_FLOWXML WHERE WFID=@WFID ";
            IDbConnection connection = DapperHelper.CreateWorkflowConnection();
            return connection.Query<WorkflowXml>(sql, new { 
                 WFID=flowID
            }).ToList().FirstOrDefault();
        }


        public static List<WorkflowXml> GetWorkflowXmlList()
        {
            string sql = " SELECT * FROM T_FLOWXML ";
            IDbConnection connection = DapperHelper.CreateWorkflowConnection();
            return connection.Query<WorkflowXml>(sql)
                   .ToList();
        }
    }
}
