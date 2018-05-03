using Smartflow.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public partial class WorkflowDesign
    {
        public TreeNode GetOrganization()
        {
            return BaseService.GetOrganization();
        }

        public IList<IEntry> GetUserByOrganizationId(string organizationId)
        {
            return BaseService.GetUserByOrganizationId(organizationId);
        }

        public IList<IEntry> GetUserByRoleId(string roleId)
        {
            return BaseService.GetUserByRoleId(roleId);
        }

        public IList<IEntry> GetRole(string roleIds)
        {
            return BaseService.GetRole(roleIds);
        }
    }
}
