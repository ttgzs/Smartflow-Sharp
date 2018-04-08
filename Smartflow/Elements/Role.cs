using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.Elements
{
    //Element
    public class Role : Element, IRelationShip
    {
        public long RNID
        {
            get;
            set;
        }

        internal override void Persistent(string instanceID)
        {
            string sql = "INSERT INTO T_ACTOR(ID,RNID,NAME) VALUES(@ID,@RNID,@NAME)";
            Connection.Execute(sql, new
            {
                RNID = RNID,
                ID = ID,
                NAME = NAME,
                INSTANCEID = instanceID
            });

        }
    }
}
