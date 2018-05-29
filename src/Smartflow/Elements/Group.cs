/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Smartflow.Enums;
using System.Xml.Serialization;

namespace Smartflow.Elements
{
    public class Group : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_GROUP(NID,ID,RNID,NAME,INSTANCEID) VALUES(@NID,@ID,@RNID,@NAME,@INSTANCEID)";
            Connection.Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                RNID = RNID,
                ID = ID,
                NAME = NAME,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
