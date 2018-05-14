/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(Node))]
    public class Transition : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        public long SOURCE
        {
            get;
            set;
        }

        [XmlAttribute("to")]
        public long DESTINATION
        {
            get;
            set;
        }

        [XmlAttribute("expression")]
        public string EXPRESSION
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_TRANSITION(NID,RNID,NAME,DESTINATION,SOURCE,INSTANCEID,EXPRESSION) VALUES(@NID,@RNID,@NAME,@DESTINATION,@SOURCE,@INSTANCEID,@EXPRESSION)";
            Connection.Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                RNID = RNID,
                NAME = NAME,
                DESTINATION = DESTINATION,
                SOURCE = SOURCE,
                INSTANCEID = INSTANCEID,
                EXPRESSION = EXPRESSION
            });
        }
    }
}
