/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.Elements
{
    public class Actor : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_ACTOR(NID,ID,RNID,NAME,INSTANCEID) VALUES(@NID,@ID,@RNID,@NAME,@INSTANCEID)";
            DapperFactory.CreateWorkflowConnection().Execute(sql, new
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
