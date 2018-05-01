using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public partial class WorkflowDesign
    {
        public IEntry GetOrganization()
        {
            return BaseService.GetOrganization();
        }

        public IList<IEntry> GetUser()
        {
            return BaseService.GetUser();
        }

        public IList<IEntry> GetUserByOrganizationId(string organizationId)
        {
            return BaseService.GetUserByOrganizationId(organizationId);
        }

        public IList<IEntry> GetUserByRoleId(string roleId)
        {
            return BaseService.GetUserByRoleId(roleId);
        }

        public IList<IEntry> GetRole()
        {
            return BaseService.GetRole();
        }
    }
}
