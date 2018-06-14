/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Smartflow.Dapper;
using Smartflow.Elements;
using Smartflow.Enums;

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

        public string Start(WorkflowStructure workflowStructure)
        {
            try
            {
                Workflow workflow = XmlConfiguration.ParseflowXml<Workflow>(workflowStructure.FILESTRUCTURE);
                List<Element> elements = new List<Element>();
                elements.Add(workflow.StartNode);
                elements.AddRange(workflow.ChildNode);
                elements.AddRange(workflow.ChildDecisionNode);
                elements.Add(workflow.EndNode);

                string instaceID = CreateWorkflowInstance(workflow.StartNode.IDENTIFICATION, workflowStructure.IDENTIFICATION, workflowStructure.JSSTRUCTURE);
                foreach (Element element in elements)
                {
                    element.INSTANCEID = instaceID;
                    element.Persistent();
                }
                return instaceID;
            }
            catch (Exception ex)
            {
                throw new WorkflowException(ex);
            }
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

        protected string CreateWorkflowInstance(long startNID, string flowID,string flowImage)
        {
            return WorkflowInstance.CreateWorkflowInstance(startNID, flowID, flowImage);
        }
    }
}
