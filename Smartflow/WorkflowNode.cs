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
    public class WorkflowNode : Node, IActor
    {
        protected WorkflowNode()
        {

        }

        public ASTNode PreviousSibling
        {
            get;
            set;
        }

        public ASTNode NextSibling
        {
            get;
            set;
        }

        #region 节点方法

        public static WorkflowNode GetWorkflowNodeInstance(ASTNode node)
        {
            WorkflowNode wfNode = new WorkflowNode();
            wfNode.NID = node.NID;
            wfNode.ID = node.ID;
            wfNode.NAME = node.NAME;
            wfNode.NodeType = node.NodeType;
            wfNode.InstanceID = node.InstanceID;
            wfNode.Transitions = wfNode.QueryWorkflowNode(node.NID);

            WorkflowProcess process = WorkflowProcess.GetWorkflowProcessInstance(wfNode.InstanceID, wfNode.NID);
            wfNode.PreviousSibling = (process == null || wfNode.NodeType == WorkflowNodeCategeory.Start) ? null :
                WorkflowNode.GetNode(wfNode.InstanceID, process.FROM);

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

        public static ASTNode GetNode(string instanceID, string ID)
        {
            string query = "SELECT * FROM T_NODE WHERE ID=@ID AND INSTANCEID=@INSTANCEID";

            ASTNode node = DapperFactory.CreateWorkflowConnection().Query<ASTNode>(query, new
            {
                ID = ID,
                INSTANCEID = instanceID

            }).FirstOrDefault();

            return node;
        }
        #endregion
    }
}
