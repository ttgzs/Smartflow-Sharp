using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public partial class WorkflowDesign
    {
        public DataTable GetOrganization()
        {
            return BaseService.GetOrganization();
        }

        public DataTable GetUser()
        {
            return BaseService.GetUser();
        }

        public DataTable GetUserByOrganizationId(string organizationId)
        {
            return BaseService.GetUserByOrganizationId(organizationId);
        }

        public DataTable GetUserByRoleId(string roleId)
        {
            return BaseService.GetUserByRoleId(roleId);
        }

        public DataTable GetRole()
        {
            return BaseService.GetRole();
        }
    }
}
