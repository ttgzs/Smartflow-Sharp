using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public class WorkflowDesign 
    {
        protected IWorkflowDesignService Context = new WorkflowDesignService();

        public WorkflowDesign()
        {

        }

        public WorkflowDesign(IWorkflowDesignService context)
        {
            this.Context = context;
        }

        public void Persistent(WorkflowXml workflowXml)
        {
            Context.Persistent(workflowXml);
        }

        public void Update(WorkflowXml workflowXml)
        {
            Context.Update(workflowXml);
        }

        public void Delete(string WFID)
        {
            Context.Delete(WFID);
        }

        public List<WorkflowXml> GetWorkflowXmlList()
        {
            return Context.GetWorkflowXmlList();
        }

        public WorkflowXml GetWorkflowXml(string WFID)
        {
            return Context.GetWorkflowXml(WFID);
        }
    }
}
