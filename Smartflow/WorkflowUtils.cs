using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Elements;
using Dapper;

namespace Smartflow
{
    public class WorkflowUtils
    {
        /// <summary>
        /// 获取所有流程节点，以供画流程图使用
        /// </summary>
        /// <returns>所有节点</returns>
        public static List<Node> GetNodeList(string instanceID)
        {
            string query = "SELECT * FROM T_NODE WHERE INSTANCEID=@INSTANCEID";
            List<Node> nodeList = DapperFactory.CreateWorkflowConnection().Query<Node>(query,new
            {
                INSTANCEID = instanceID

            }).ToList();

            foreach (Node n in nodeList)
            {
                n.Transitions = n.QueryWorkflowNode(n.NID);
            }
            
            return nodeList;
        }
    }
}
