/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen
 Email:237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Xml.Serialization;

namespace Smartflow.Elements
{
    public class Rule : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        [XmlAttribute("to")]
        public long TO
        {
            get;
            set;
        }

        [XmlAttribute("expression")]
        public string Expression
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_RULE(NID,RNID,NAME,[TO],EXPRESSION,INSTANCEID) VALUES(@NID,@RNID,@NAME,@TO,@EXPRESSION,@INSTANCEID)";

            Connection.Execute(sql, new
            {
                NID=Guid.NewGuid().ToString(),
                RNID = RNID,
                NAME = NAME,
                TO = TO,
                EXPRESSION = Expression,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
