/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Smartflow.Design;
using Smartflow.Infrastructure;
using Dapper;
using Smartflow.Web.Models;

namespace Smartflow.Integration
{
    public class WorkflowInfrastructure : IInfrastructure
    {
        public IDbConnection Connection
        {
            get { return DapperHelper.CreateWorkflowConnection(); }
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