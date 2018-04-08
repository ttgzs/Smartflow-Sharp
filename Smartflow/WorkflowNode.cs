using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Enums;

namespace Smartflow
{
    public class WorkflowNode :Node,IActor
    {
        protected WorkflowNode()
        {

        }

        public Node PreviousSibling
        {
            get;
            set;
        }

        public Node NnextSibling
        {
            get;
            set;
        }

        #region 节点方法

        public static WorkflowNode GetWorkflowNodeInstance(ASTNode node)
        {
            WorkflowNode wfNode = new WorkflowNode();
            wfNode.ID = node.ID;
            wfNode.NAME = node.NAME;
            wfNode.NodeType= node.NodeType;
            wfNode.Transitions = wfNode.QueryWorkflowNode(node.NID);
            return wfNode;
        }

        public List<Actor> GetActorList()
        {
            string query = "SELECT * FROM T_ACTOR WHERE RNID=@RNID";

            return Connection.Query<Actor>(query, new { RNID = NID })
                  .ToList();
        }

        public bool CheckActor(long actorID)
        {
            if (NodeType == WorkflowNodeCategeory.Start || NodeType == WorkflowNodeCategeory.Decision) return true;

            string sql = "SELECT Count(*) FROM T_ACTOR WHERE ID=@ID AND RNID=@RNID";

            IList<Actor> actors = GetActorListByRole();

            return Connection.ExecuteScalar<int>(sql, new
            {
                RNID = NID,
                ID = actorID

            }) > 0 || actors.Count(actor => actor.ID == actorID.ToString()) > 0;
        }

        public List<Actor> GetActorListByRole()
        {
            return new List<Actor>();
        }
        #endregion
    }
}
