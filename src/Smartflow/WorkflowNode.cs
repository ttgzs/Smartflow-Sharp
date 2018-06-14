/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Smartflow.Dapper;
using Smartflow.Elements;
using Smartflow.Enums;

namespace Smartflow
{
    public class WorkflowNode : Node
    {
        protected WorkflowNode()
        {

        }

        public List<Transition> GetTransitions()
        {
            foreach (Smartflow.Elements.Transition transition in this.Transitions)
            {
                ASTNode an = this.GetNode(transition.DESTINATION);
                Transition decisionTransition = transition;
                while (an.NodeType == Enums.WorkflowNodeCategeory.Decision)
                {
                    WorkflowDecision decision = WorkflowDecision.ConvertToReallyType(an);
                    decisionTransition = decision.GetTransition();
                    an = this.GetNode(decisionTransition.DESTINATION);
                }
                transition.APPELLATION = decisionTransition.APPELLATION;
            }
            return this.Transitions;
        }

        /// <summary>
        /// 上一个执行跳转路线
        /// </summary>
        public Transition FromTransition
        {
            get;
            set;
        }

        #region 节点方法

        public static WorkflowNode ConvertToReallyType(ASTNode node)
        {
            WorkflowNode wfNode = new WorkflowNode();
            wfNode.NID = node.NID;
            wfNode.IDENTIFICATION = node.IDENTIFICATION;
            wfNode.APPELLATION = node.APPELLATION;
            wfNode.NodeType = node.NodeType;
            wfNode.INSTANCEID = node.INSTANCEID;
            wfNode.Transitions = wfNode.QueryWorkflowNode(node.NID);
            wfNode.FromTransition = wfNode.GetHistoryTransition();
            wfNode.Groups = wfNode.GetGroup();
            return wfNode;
        }

        /// <summary>
        /// 上一个执行跳转节点
        /// </summary>
        /// <returns></returns>
        public WorkflowNode GetFromNode()
        {
            if (FromTransition == null) return null;
            ASTNode node = GetNode(FromTransition.ORIGIN);
            return WorkflowNode.ConvertToReallyType(node);    
        }

        public List<Actor> GetActors()
        {
            string query = " SELECT * FROM T_ACTOR WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID AND OPERATION=@OPERATION ";
            return Connection.Query<Actor>(query, new
            {
                RNID = NID,
                INSTANCEID = INSTANCEID,
                OPERATION = WorkflowAction.Jump

            }).ToList();
        }

        /// <summary>
        /// 获取回退线路
        /// </summary>
        /// <returns>路线</returns>
        protected Transition GetHistoryTransition()
        {
            Transition transition = null;
            try
            {
                WorkflowProcess process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, NID);
                if (process != null && NodeType != WorkflowNodeCategeory.Start)
                {
                    ASTNode n = GetNode(process.ORIGIN);
                    while (n.NodeType == WorkflowNodeCategeory.Decision)
                    {
                        process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, n.NID);
                        n = GetNode(process.ORIGIN);

                        if (n.NodeType == WorkflowNodeCategeory.Start)
                            break;
                    }
                    transition = GetTransition(process.TRANSITIONID);
                }
            }
            catch (Exception ex)
            {
                throw new WorkflowException(ex, INSTANCEID);
            }
            return transition;
        }

        /// <summary>
        /// 依据主键获取路线
        /// </summary>
        /// <param name="TRANSITIONID">路线主键</param>
        /// <returns>路线</returns>
        protected Transition GetTransition(string TRANSITIONID)
        {
            string query = "SELECT * FROM T_TRANSITION WHERE NID=@TRANSITIONID AND INSTANCEID=@INSTANCEID";
            Transition transition = Connection.Query<Transition>(query, new
            {
                TRANSITIONID = TRANSITIONID,
                INSTANCEID = INSTANCEID

            }).FirstOrDefault();

            return transition;
        }

        protected List<Group> GetGroup()
        {
            string query = "SELECT * FROM T_GROUP WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID";
            return Connection.Query<Group>(query, new
            {
                RNID = NID,
                INSTANCEID = INSTANCEID

            }).ToList();
        }

        /// <summary>
        /// 获取当前执行的跳转路线
        /// </summary>
        /// <param name="transitionID">跳转ID</param>
        /// <returns>跳转对象</returns>
        protected Transition GetExecuteTransition(string transitionID)
        {
            Transition executeTransition = Transitions
                .FirstOrDefault(t => t.NID == transitionID);

            ASTNode an = this.GetNode(executeTransition.DESTINATION);
            Transition returnTransition = executeTransition;
            while (an.NodeType == Enums.WorkflowNodeCategeory.Decision)
            {
                WorkflowDecision decision = WorkflowDecision.ConvertToReallyType(an);
                returnTransition = decision.GetTransition();
                an = this.GetNode(returnTransition.DESTINATION);
            }
            return returnTransition;
        }

        /// <summary>
        /// 获取下一组
        /// </summary>
        /// <param name="transitionID">跳转ID</param>
        /// <returns></returns>
        public List<Group> GetNextGroup(string transitionID)
        {
            Transition executeTransition = this.GetExecuteTransition(transitionID);
            ASTNode selectNode = this.GetNode(executeTransition.DESTINATION);

            string query = "SELECT * FROM T_GROUP WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID";
            return Connection.Query<Group>(query, new
            {
                RNID = selectNode.NID,
                INSTANCEID = INSTANCEID

            }).ToList();
        }
        #endregion
    }
}
