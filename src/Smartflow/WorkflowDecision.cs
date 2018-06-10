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
        public static WorkflowDecision ConvertToReallyType(ASTNode node)
        {
            WorkflowDecision wfNode = new WorkflowDecision();
            wfNode.INSTANCEID = node.INSTANCEID;
            wfNode.NID = node.NID;
            wfNode.IDENTIFICATION = node.IDENTIFICATION;
            wfNode.APPELLATION = node.APPELLATION;
            wfNode.NodeType = node.NodeType;
            return wfNode;
        }

        /// <summary>
        /// 动态获取路线，根据决策节点设置条件表达式，自动去判断流转的路线
        /// </summary>
        /// <returns>路线</returns>
        public Transition GetTransition()
        {
            Command CMD = GetExecuteCmd();
            IDbConnection connect = DapperFactory.CreateConnection(CMD.DBCATEGORY, CMD.CONNECTE);
            try
            {
                DataTable resultSet = new DataTable(Guid.NewGuid().ToString());
                using (IDataReader reader = connect.ExecuteReader(CMD.SCRIPT, new { INSTANCEID = INSTANCEID }))
                {
                    resultSet.Load(reader);
                    reader.Close();
                }

                Transition instance = null;
                List<Transition> transitions = QueryWorkflowNode(NID);
                if (resultSet.Rows.Count > 0)
                {
                    foreach (Transition transition in transitions)
                    {
                        if (resultSet.Select(transition.EXPRESSION).Length > 0)
                        {
                            instance = transition;
                            break;
                        }
                    }
                }
                return instance;
            }
            catch (Exception ex)
            {
                throw new WorkflowException(ex);
            }
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
    }
}
