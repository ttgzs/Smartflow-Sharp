using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.Elements
{
    public class Actor : Element, IRelationShip
    {
        public long RNID
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_ACTOR(ID,RNID,NAME,INSTANCEID) VALUES(@ID,@RNID,@NAME,@INSTANCEID)";
            DapperFactory.CreateWorkflowConnection().Execute(sql, new
            {
                RNID = RNID,
                ID = ID,
                NAME = NAME,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
