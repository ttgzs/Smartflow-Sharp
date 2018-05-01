using Smartflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public partial class WorkflowDesign
    {
        protected IWorkflowDesignService Context = new WorkflowDesignService();
        protected IInfrastructure BaseService = null;

        public WorkflowDesign()
        {

        }

        public WorkflowDesign(IWorkflowDesignService context)
        {
            this.Context = context;
        }

        public WorkflowDesign(IInfrastructure dataService)
        {
            this.BaseService = dataService;
        }

        public WorkflowDesign(IWorkflowDesignService context, IInfrastructure dataService)
        {
            this.Context = context;
            this.BaseService = dataService;
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
