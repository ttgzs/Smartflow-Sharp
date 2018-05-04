using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Infrastructure;
using Smartflow.Integration.Models;

namespace Smartflow.Integration
{
    public class WorkflowDesign
    {
        protected IWorkflowDesignService Context = new WorkflowDesignService();

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

        public TreeNode GetOrganization()
        {
            return null;
        }

        public IList<IEntry> GetUserByOrganizationId(string organizationId)
        {
            return null;
        }

        public IList<IEntry> GetUserByRoleId(string roleId)
        {
            return null;
        }

        public IList<IEntry> GetRole(string roleIds)
        {
            return null;
        }
    }
}
