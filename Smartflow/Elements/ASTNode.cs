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


        internal override void Persistent(string instanceID)
        {
            string sql = "INSERT INTO T_NODE(ID,NAME,NODETYPE,INSTANCEID) VALUES(@ID,@NAME,@NODETYPE,@INSTANCEID);SELECT @@IDENTITY";
            this.NID = Connection.ExecuteScalar<long>(sql, new
            {
                ID = ID,
                NAME = NAME,
                NODETYPE = NodeType.ToString(),
                INSTANCEID = instanceID
            });
        }


        internal virtual List<Transition> QueryWorkflowNode(long nodeID)
        {
            IDbConnection connection = Connection;
            string query = "SELECT * FROM T_TRANSITION WHERE RNID=@RNID";

            return connection.Query<Transition>(query, new { RNID = nodeID })
                  .ToList();
        }
    }
}
