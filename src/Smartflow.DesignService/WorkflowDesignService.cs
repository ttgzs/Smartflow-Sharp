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
            string sql = " INSERT INTO T_FLOWXML(IDENTIFICATION,APPELLATION,XML,IMAGE) VALUES(@IDENTIFICATION,@APPELLATION,@XML,@IMAGE) ";
            Connection.Execute(sql, workflowXml);
        }

        public void Update(WorkflowXml workflowXml)
        {
            string sql = " UPDATE T_FLOWXML SET APPELLATION=@APPELLATION,XML=@XML,IMAGE=@IMAGE WHERE IDENTIFICATION=@IDENTIFICATION ";
            Connection.Execute(sql, workflowXml);
        }

        public WorkflowXml GetWorkflowXml(string IDENTIFICATION)
        {
            string sql = " SELECT * FROM T_FLOWXML WHERE IDENTIFICATION=@IDENTIFICATION ";
            return Connection.Query<WorkflowXml>(sql, new { IDENTIFICATION = IDENTIFICATION })
                .FirstOrDefault<WorkflowXml>();
        }
    }
}
