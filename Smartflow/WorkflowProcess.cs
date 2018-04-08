using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Enums;
using System.Data;

namespace Smartflow
{
    public class WorkflowProcess : IPersistent
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
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
            string sql = "INSERT INTO T_PROCESS([FROM],[TO],TID,INSTANCEID,NODETYPE) VALUES(@FROM,@TO,@TID,@INSTANCEID,@NODETYPE)";
            Connection.Execute(sql, new
            {
                FROM = FROM,
                TO = TO,
                TID = TID,
                INSTANCEID = INSTANCEID,
                NODETYPE = NODETYPE.ToString()
            });
        }
    }
}
