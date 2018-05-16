using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Infrastructure;

namespace Smartflow.DesignService
{
    public partial class WorkflowDesignService
    {
        protected IDbConnection Connection
        {
            get { return SqlHelper.CreateConnection(); }
        }

        public void Persistent(WorkflowXml workflowXml)
        {
            string sql = "INSERT INTO T_FLOWXML(WFID,NAME,XML,IMAGE)  VALUES(@WFID,@NAME,@XML,@IMAGE)";
            Connection.Execute(sql, workflowXml);
        }

        public void Update(WorkflowXml workflowXml)
        {
            string sql = " UPDATE T_FLOWXML SET NAME=@NAME,XML=@XML,IMAGE=@IMAGE WHERE WFID=@WFID ";
            Connection.Execute(sql, workflowXml);
        }

        //public void Delete(string WFID)
        //{
        //    string sql = " DELETE FROM T_FLOWXML WHERE WFID=@WFID ";
        //    Connection.Execute(sql, new
        //    {
        //        WFID = WFID
        //    });
        //}

        //public List<WorkflowXml> GetWorkflowXmlList()
        //{
        //    string sql = " SELECT * FROM T_FLOWXML ";
        //    return Connection.Query<WorkflowXml>(sql).ToList();
        //}

        public WorkflowXml GetWorkflowXml(string WFID)
        {
            string sql = "SELECT * FROM T_FLOWXML WHERE WFID=@WFID";

            return Connection.Query<WorkflowXml>(sql, new { WFID = WFID })
                .FirstOrDefault<WorkflowXml>();
        }
    }
}
