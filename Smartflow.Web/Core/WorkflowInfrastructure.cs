using Smartflow.Design;
using Smartflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartflow.Web.Core
{
    public class WorkflowInfrastructure : IInfrastructure
    {
        public TreeNode GetOrganization()
        {
            return null;
        }

        public IList<IEntry> GetUser()
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

        public IList<IEntry> GetRole()
        {
            return null;
        }
    }
}