/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Elements;
using Smartflow.Enums;
using Smartflow.Infrastructure;

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
            string instaceID = CreateWorkflowInstance(workflow.StartNode.ID, workflowXml.WFID, workflowXml.IMAGE);
            foreach (Element element in elements)
            {
                element.INSTANCEID = instaceID;
                element.Persistent();
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

   

        protected string CreateWorkflowInstance(long startNID, string flowID,string flowImage)
        {
            return WorkflowInstance.CreateWorkflowInstance(startNID, flowID, flowImage);
        }
    }
}
