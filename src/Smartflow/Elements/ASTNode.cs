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
using System.Data;

using Dapper;
using Smartflow.Enums;


namespace Smartflow.Elements
{
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
            string sql = "INSERT INTO T_NODE(NID,IDENTIFICATION,APPELLATION,NODETYPE,INSTANCEID) VALUES(@NID,@IDENTIFICATION,@APPELLATION,@NODETYPE,@INSTANCEID)";
            Connection.ExecuteScalar<long>(sql, new
            {
                NID = NID,
                IDENTIFICATION = IDENTIFICATION,
                APPELLATION = APPELLATION,
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

   

        /// <summary>
        /// 记录已经参与过审核人员的信息
        /// </summary>
        /// <param name="actorID"></param>
        /// <param name="actorName"></param>
        /// <param name="action"></param>
        internal virtual void SetActor(long actorID,string actorName,WorkflowAction action)
        {
            if (this.NodeType != WorkflowNodeCategeory.Decision)
            {
                Actor actor = new Actor();
                actor.IDENTIFICATION = actorID;
                actor.APPELLATION = actorName;
                actor.RNID = NID;
                actor.OPERATION = action;
                actor.INSTANCEID = INSTANCEID;
                actor.Persistent();
            }
        }

    }
}
