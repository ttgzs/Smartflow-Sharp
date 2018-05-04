using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Integration.Models;

namespace Smartflow.Integration
{
    public partial class WorkflowDesignService
    {
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
            List<IEntry> entry = new List<IEntry>();

            string query = " SELECT * FROM T_ROLE WHERE 1=1 ";
            if (!String.IsNullOrEmpty(roleIds))
            {
                query = query + " AND EID NOT IN (" + roleIds + ")";
            }

            entry.AddRange(Connection.Query<Role>(query).ToList());
            return entry;
        }
    }
}
