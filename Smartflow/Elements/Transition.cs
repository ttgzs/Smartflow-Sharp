using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dapper;

namespace Smartflow.Elements
{
    //Element
    [XmlInclude(typeof(Node))]
    public class Transition : Element, IRelationShip
    {
        public long RNID
        {
            get;
            set;
        }

        public long FROM
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

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_TRANSITION(ID,RNID,NAME,[TO],[FROM],INSTANCEID) VALUES(@ID,@RNID,@NAME,@TO,@FROM,@INSTANCEID)";
            Connection.Execute(sql, new
            {
                ID = ID,
                RNID = RNID,
                NAME = NAME,
                TO = TO,
                FROM=FROM,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
