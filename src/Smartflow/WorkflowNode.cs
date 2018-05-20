/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Elements;
using Smartflow.Enums;

namespace Smartflow
{
    public class WorkflowNode : Node
    {
        protected WorkflowNode()
        {

        }

        public Transition Previous
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
            wfNode.INSTANCEID = node.INSTANCEID;
            wfNode.Transitions = wfNode.QueryWorkflowNode(node.NID);
            wfNode.Previous = wfNode.GetHistoryTransition();
            wfNode.Groups = wfNode.GetGroups();
            return wfNode;
        }

        public WorkflowNode GetPreviousNode()
        {
            if (Previous == null) return null;
            return GetWorkflowNodeInstance(this.GetNode(Previous.SOURCE));
        }

        /// <summary>
        /// 获取回退线路
        /// </summary>
        /// <returns>路线</returns>
        protected Transition GetHistoryTransition()
        {
            Transition transition = null;
            WorkflowProcess process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, NID);
            if (process != null || NodeType != WorkflowNodeCategeory.Start)
            {
                ASTNode n = GetNode(process.SOURCE);
                while (n.NodeType == WorkflowNodeCategeory.Decision)
                {
                    process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, n.NID);
                    n = GetNode(process.SOURCE);

                    if (n.NodeType == WorkflowNodeCategeory.Start)
                        break;
                }
                transition = GetTransition(process.TID);
            }

            return transition;
        }

        /// <summary>
        /// 依据主键获取路线
        /// </summary>
        /// <param name="TID">路线主键</param>
        /// <returns>路线</returns>
        protected Transition GetTransition(string TID)
        {
            string query = "SELECT * FROM T_TRANSITION WHERE NID=@TID AND INSTANCEID=@INSTANCEID";
            Transition transition = Connection.Query<Transition>(query, new
            {
                TID = TID,
                INSTANCEID = INSTANCEID

            }).FirstOrDefault();

            return transition;
        }

        protected List<Group> GetGroups()
        {
            string query = "SELECT * FROM T_GROUP WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID";
            return Connection.Query<Group>(query, new
            {
                RNID = NID,
                INSTANCEID = INSTANCEID

            }).ToList();
        }
        #endregion
    }
}
