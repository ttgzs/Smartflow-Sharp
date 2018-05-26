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
            wfNode.ID = node.ID;
            wfNode.NAME = node.NAME;
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
            ASTNode node = GetNode(FromTransition.SOURCE);
            return WorkflowNode.ConvertToReallyType(node);
        }

        public List<Actor> GetActors()
        {
            string query = " SELECT * FROM T_ACTOR WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID ";

            return Connection.Query<Actor>(query, new
            {
                RNID = NID,
                INSTANCEID = INSTANCEID

            }).ToList();
        }

        /// <summary>
        /// 获取回退线路
        /// </summary>
        /// <returns>路线</returns>
        protected Transition GetHistoryTransition()
        {
            Transition transition = null;
            WorkflowProcess process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, NID);
            if (process != null && NodeType != WorkflowNodeCategeory.Start)
            {
                ASTNode n = GetNode(process.SOURCE);
                while (n.NodeType == WorkflowNodeCategeory.Decision)
                {
                    process = WorkflowProcess.GetWorkflowProcessInstance(INSTANCEID, n.NID);
                    n = GetNode(process.SOURCE);

                    if (n.NodeType == WorkflowNodeCategeory.Start)
                        break;
                }
                transition = GetTransition(process.TRANSITIONID);
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
        /// 获取下一组
        /// </summary>
        /// <param name="nextNID">相邻的节点ID</param>
        /// <returns></returns>
        public List<Group> GetNextGroup(string nextNID)
        {
            string query = "SELECT * FROM T_GROUP WHERE RNID=@RNID AND INSTANCEID=@INSTANCEID";
            return Connection.Query<Group>(query, new
            {
                RNID = nextNID,
                INSTANCEID = INSTANCEID

            }).ToList();
        }
        #endregion
    }
}
