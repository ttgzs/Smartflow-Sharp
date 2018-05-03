/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;

namespace Smartflow.Infrastructure
{
    public interface IInfrastructure
    {
        /// <summary>
        /// 定义获取所有组织机构
        /// </summary>
        /// <returns></returns>
        TreeNode GetOrganization();

        /// <summary>
        /// 依据组织机构标识，获取用户信息
        /// </summary>
        /// <param name="organizationId">组织机构标识</param>
        /// <returns></returns>
        IList<IEntry> GetUserByOrganizationId(string organizationId);

        /// <summary>
        /// 依据角色标识，获取用户信息
        /// </summary>
        /// <param name="roleId">角色标识</param>
        /// <returns></returns>
        IList<IEntry> GetUserByRoleId(string roleId);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="roleIds">角色ID</param>
        /// <returns></returns>
        IList<IEntry> GetRole(string roleIds);
    }
}
