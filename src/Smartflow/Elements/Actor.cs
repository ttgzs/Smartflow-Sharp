/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    public class Actor : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        public WorkflowAction OPERATION
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_ACTOR(NID,ID,RNID,NAME,INSTANCEID,OPERATION) VALUES(@NID,@ID,@RNID,@NAME,@INSTANCEID,@OPERATION)";
            DapperFactory.CreateWorkflowConnection().Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                RNID = RNID,
                ID = ID,
                NAME = NAME,
                INSTANCEID = INSTANCEID,
                OPERATION = OPERATION
            });
        }
    }
}
