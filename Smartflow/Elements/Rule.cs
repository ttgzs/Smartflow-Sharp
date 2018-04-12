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
        public long RNID
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
            string sql = "INSERT INTO T_RULE(RNID,NAME,[TO],EXPRESSION,INSTANCEID) VALUES(@RNID,@NAME,@TO,@EXPRESSION,@INSTANCEID)";

            Connection.Execute(sql, new
            {
                RNID = RNID,
                NAME = NAME,
                TO = TO,
                EXPRESSION = Expression,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
