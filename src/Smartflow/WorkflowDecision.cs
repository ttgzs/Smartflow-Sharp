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

namespace Smartflow
{
    /// <summary>
    /// 扩展决策节点，并对外提供服务
    /// </summary>
    public class WorkflowDecision : Decision, ITransition
    {
        /// <summary>
        /// 获取决策节点实例 
        /// </summary>
        /// <param name="node">抽象节点</param>
        /// <returns>决策节点实例</returns>
        public static WorkflowDecision GetNodeInstance(ASTNode node)
        {
            WorkflowDecision wfNode = new WorkflowDecision();
            wfNode.INSTANCEID = node.INSTANCEID;
            wfNode.NID = node.NID;
            wfNode.ID = node.ID;
            wfNode.NAME = node.NAME;
            wfNode.NodeType = node.NodeType;
            return wfNode;
        }

        /// <summary>
        /// 动态获取路线，根据决策节点设置条件表达式，自动去判断流转的路线
        /// </summary>
        /// <returns>路线</returns>
        public Transition GetTransition()
        {
            Command cmd = GetExecuteCmd();
            List<Smartflow.Elements.Rule> rules = GetRuleList();

            IDbConnection connect = DapperFactory.CreateConnection(cmd.DBCATEGORY, cmd.CONNECTION);
            DataTable resultSet = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader reader = connect.ExecuteReader(cmd.Text, new { INSTANCEID = INSTANCEID }))
            {
                resultSet.Load(reader);
                reader.Close();
            }

            long transitionID =0;
            if (resultSet.Rows.Count > 0)
            {
                foreach (Smartflow.Elements.Rule rule in rules)
                {
                    if (resultSet.Select(rule.Expression).Length > 0)
                    {
                        transitionID = rule.TO;
                        break;
                    }
                }
            }

            List<Transition> transitions =QueryWorkflowNode(NID);
            return transitions.FirstOrDefault(t => t.ID == transitionID);
        }

        /// <summary>
        /// 获取执行SQL命令的对象
        /// </summary>
        /// <returns>SQL命令的对象</returns>
        protected Command GetExecuteCmd()
        {
            string query = "SELECT * FROM T_COMMAND WHERE RNID=@RNID";

            return Connection.Query<Command>(query, new { RNID = NID })
                  .FirstOrDefault();
        }

        /// <summary>
        /// 获取规则列表，规则中定义表达式，并指定跳转跳线
        /// </summary>
        /// <returns>规则列表</returns>
        protected List<Smartflow.Elements.Rule> GetRuleList()
        {
            string query = "SELECT * FROM T_RULE WHERE RNID=@RNID";

            return Connection.Query<Smartflow.Elements.Rule>(query, new { RNID = NID })
                  .ToList();
        }
    }
}
