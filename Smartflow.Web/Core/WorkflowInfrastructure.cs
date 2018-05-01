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

namespace Smartflow.Web.Core
{
    public class WorkflowInfrastructure : IInfrastructure
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

        public IList<IEntry> GetRole()
        {
            return null;
        }
    }
}