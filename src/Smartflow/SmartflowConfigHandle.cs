/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 定义工作流数据库配置节点
    /// </summary>
    public class SmartflowConfigHandle : ConfigurationSection
    {
        /// <summary>
        /// 工作流数据库连接字符串
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return this["connectionString"].ToString(); }
        }
        
        /// <summary>
        /// 工作流数据库类别
        /// </summary>
        [ConfigurationProperty("category", IsRequired = true, DefaultValue = "SQLServer")]
        public string DatabaseCategory
        {
            get { return this["category"].ToString(); }
        }
    }
}
