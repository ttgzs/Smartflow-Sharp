/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
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

        public long ORIGIN
        {
            get;
            set;
        }

        [XmlAttribute("destination")]
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
            string sql = "INSERT INTO T_TRANSITION(NID,RNID,APPELLATION,DESTINATION,ORIGIN,INSTANCEID,EXPRESSION) VALUES(@NID,@RNID,@APPELLATION,@DESTINATION,@ORIGIN,@INSTANCEID,@EXPRESSION)";
            Connection.Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                RNID = RNID,
                APPELLATION = APPELLATION,
                DESTINATION = DESTINATION,
                ORIGIN = ORIGIN,
                INSTANCEID = INSTANCEID,
                EXPRESSION = EXPRESSION
            });
        }
    }
}
