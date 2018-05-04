using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Integration.Models;

namespace Smartflow.Integration
{
    public interface IWorkflowDesignService
    {
        void Persistent(WorkflowXml workflowXml);
        void Update(WorkflowXml workflowXml);
        void Delete(string WFID);
        List<WorkflowXml> GetWorkflowXmlList();
        WorkflowXml GetWorkflowXml(string WFID);
    }
}
