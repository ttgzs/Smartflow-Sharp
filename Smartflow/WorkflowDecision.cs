using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Elements;

namespace Smartflow
{
    public class WorkflowDecision : Decision, ITransition
    {
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

        public Transition GetTransition(string instanceID)
        {
            Command cmd = GetExecuteCmd();
            List<Smartflow.Elements.Rule> rules = GetRuleList();

            IDbConnection connect = DapperFactory.CreateConnection(cmd.DBCATEGORY, cmd.CONNECTION);
            DataTable resultSet = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader reader = connect.ExecuteReader(cmd.Text, new { INSTANCEID = instanceID }))
            {
                resultSet.Load(reader);
                reader.Close();
            }
            string transitionID = string.Empty;
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
            List<Transition> transitions =QueryWorkflowNode(this.NID);
            return transitions.FirstOrDefault(t => t.ID == transitionID);
        }

        protected Command GetExecuteCmd()
        {
            string query = "SELECT * FROM T_COMMAND WHERE RNID=@RNID";

            return Connection.Query<Command>(query, new { RNID = NID })
                  .FirstOrDefault();
        }

        protected List<Smartflow.Elements.Rule> GetRuleList()
        {
            string query = "SELECT * FROM T_RULE WHERE RNID=@RNID";

            return Connection.Query<Smartflow.Elements.Rule>(query, new { RNID = NID })
                  .ToList();
        }
    }
}
