using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow
{
    public class WorkflowDesignService : IWorkflowDesignService
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
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

        public void Delete(string IDENTIFICATION)
        {
            string sql = " DELETE FROM T_STRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION ";
            Connection.Execute(sql, new { IDENTIFICATION = IDENTIFICATION });
        }

        public List<WorkflowStructure> GetWorkflowStructureList()
        {
            string sql = " SELECT * FROM T_STRUCTURE ";
            return Connection.Query<WorkflowStructure>(sql).ToList();
        }

        public WorkflowStructure GetWorkflowStructure(string identification)
        {
            string sql = " SELECT * FROM T_STRUCTURE WHERE IDENTIFICATION=@IDENTIFICATION ";
            return DapperFactory.CreateWorkflowConnection().Query<WorkflowStructure>(sql, new { IDENTIFICATION = identification })
                .FirstOrDefault<WorkflowStructure>();
        }
    }
}
