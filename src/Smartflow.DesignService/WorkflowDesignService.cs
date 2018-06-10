/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 */
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

        public void Persistent(WorkflowStructure workflowStructure)
        {
            string sql = " INSERT INTO T_STRUCTURE(IDENTIFICATION,APPELLATION,FILESTRUCTURE,JSSTRUCTURE) VALUES(@IDENTIFICATION,@APPELLATION,@FILESTRUCTURE,@JSSTRUCTURE) ";
            Connection.Execute(sql, workflowStructure);
        }

        public void Update(WorkflowStructure workflowStructure)
        {
            string sql = " UPDATE T_STRUCTURE SET APPELLATION=@APPELLATION,FILESTRUCTURE=@FILESTRUCTURE,JSSTRUCTURE=@JSSTRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION ";
            Connection.Execute(sql, workflowStructure);
        }

        public WorkflowStructure GetWorkflowStructure(string IDENTIFICATION)
        {
            string sql = " SELECT * FROM T_STRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION ";
            return Connection.Query<WorkflowStructure>(sql, new { IDENTIFICATION = IDENTIFICATION })
                .FirstOrDefault<WorkflowStructure>();
        }
    }
}
