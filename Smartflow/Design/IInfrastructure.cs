using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public interface IInfrastructure
    {
        /// <summary>
        /// 定义获取所有组织机构
        /// </summary>
        /// <returns></returns>
        TreeNode GetOrganization();
        
        /// <summary>
        /// 获取有用户
        /// </summary>
        /// <returns></returns>
        IList<IEntry> GetUser();

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
        /// <returns></returns>
        IList<IEntry> GetRole();
    }
}
