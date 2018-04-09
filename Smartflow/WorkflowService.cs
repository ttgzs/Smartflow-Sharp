using Smartflow.Elements;
using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow
{
    public partial class WorkflowService : IWorkflow
    {
        private IDbConnection conn = DapperFactory.CreateWorkflowConnection();

        public WorkflowInstance Instance(string instanceID)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            return instance;
        }

        public string Start(WorkflowXml workflowXml)
        {
            Workflow workflow = XmlConfiguration.ParseflowXml<Workflow>(workflowXml.XML);
            List<Element> elements = new List<Element>();
            elements.Add(workflow.StartNode);
            elements.AddRange(workflow.ChildNode);

            elements.AddRange(workflow.ChildDecisionNode);

            elements.Add(workflow.EndNode);

            //创建关联实例
            string instaceID = CreateWorkflowInstance(workflow.StartNode.ID, workflowXml.ID);
            foreach (Element element in elements)
            {
                element.Persistent(instaceID);
            }
            return instaceID;
        }

  

        public void Kill(WorkflowInstance instance)
        {
            if (instance.State == WorkflowInstanceState.Running)
            {
                instance.State = WorkflowInstanceState.Kill;
                instance.Transfer();
            }
        }

        public void Terminate(WorkflowInstance instance)
        {
            if (instance.State == WorkflowInstanceState.Running)
            {
                instance.State = WorkflowInstanceState.Termination;
                instance.Transfer();
            }
        }

        public void Revert(WorkflowInstance instance)
        {
            if (instance.State == WorkflowInstanceState.Termination)
            {
                instance.State = WorkflowInstanceState.Running;
                instance.Transfer();
            }
        }

   

        protected string CreateWorkflowInstance(string startNID, long flowID)
        {
            return WorkflowInstance.CreateWorkflowInstance(startNID, flowID);
        }
    }
}
