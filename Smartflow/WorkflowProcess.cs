using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Enums;
using System.Data;

namespace Smartflow
{
    public class WorkflowProcess : IPersistent, IRelationShip
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
        }

        public long RNID
        {
            get;
            set;
        }

        public long NID
        {
            get;
            set;
        }

        public string FROM
        {
            get;
            set;
        }

        public string TO
        {
            get;
            set;
        }

        public long TID
        {
            get;
            set;
        }

        public string INSTANCEID
        {
            get;
            set;
        }

        public WorkflowNodeCategeory NODETYPE
        {
            get;
            set;
        }

        public void Persistent()
        {
            string sql = "INSERT INTO T_PROCESS([FROM],[TO],TID,INSTANCEID,NODETYPE,RNID) VALUES(@FROM,@TO,@TID,@INSTANCEID,@NODETYPE,@RNID)";
            Connection.Execute(sql, new
            {
                FROM = FROM,
                TO = TO,
                TID = TID,
                INSTANCEID = INSTANCEID,
                NODETYPE = NODETYPE.ToString(),
                RNID = RNID
            });
        }


        public static WorkflowProcess GetWorkflowProcessInstance(string instanceID, long NID)
        {
            WorkflowProcess instance = new WorkflowProcess();
            string query = " SELECT TOP 1 * FROM T_PROCESS WHERE INSTANCEID=@INSTANCEID AND RNID=@NID ORDER BY CREATEDATE DESC ";
            instance = instance.Connection.Query<WorkflowProcess>(query, new
            {
                INSTANCEID = instanceID,
                NID = NID

            }).FirstOrDefault();

            return instance;
        }
    }
}
