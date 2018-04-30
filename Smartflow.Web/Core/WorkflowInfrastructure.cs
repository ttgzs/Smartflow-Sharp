using Smartflow.Design;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartflow.Web.Core
{
    public class WorkflowInfrastructure : IInfrastructure
    {
        public DataTable GetOrganization()
        {
            return null;
        }

        public DataTable GetUser()
        {
            return null;
        }

        public DataTable GetUserByOrganizationId(string organizationId)
        {
            return null;
        }

        public DataTable GetUserByRoleId(string roleId)
        {
            return null;
        }

        public DataTable GetRole()
        {
            return null;
        }
    }
}