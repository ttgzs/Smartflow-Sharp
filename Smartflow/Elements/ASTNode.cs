using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dapper;
using Smartflow.Enums;
using System.Data;

namespace Smartflow.Elements
{
    //Element->ASTNode
    public class ASTNode : Element
    {
        [XmlElement(ElementName = "transition")]
        public virtual List<Transition> Transitions
        {
            get;
            set;
        }

        [XmlIgnore]
        public virtual WorkflowNodeCategeory NodeType
        {
            get;
            set;
        }


        internal override void Persistent()
        {
            NID = Guid.NewGuid().ToString();
            string sql = "INSERT INTO T_NODE(NID,ID,NAME,NODETYPE,INSTANCEID) VALUES(@NID,@ID,@NAME,@NODETYPE,@INSTANCEID)";
            Connection.ExecuteScalar<long>(sql, new
            {
                NID=NID,
                ID = ID,
                NAME = NAME,
                NODETYPE = NodeType.ToString(),
                INSTANCEID = INSTANCEID
            });
        }


        internal virtual List<Transition> QueryWorkflowNode(string NID)
        {
            IDbConnection connection = Connection;
            string query = "SELECT * FROM T_TRANSITION WHERE RNID=@RNID";

            return connection.Query<Transition>(query, new { RNID = NID })
                  .ToList();
        }

        internal virtual void SetActor(List<Actor> actors)
        {
            foreach (Actor actor in actors)
            {
                actor.RNID = NID;
                actor.INSTANCEID = INSTANCEID;
                actor.Persistent();
            }
        }
    }
}
