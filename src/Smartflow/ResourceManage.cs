/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 处理比较复杂的SQL语句、异常、工作流信息
    /// </summary>
    internal class ResourceManage
    {
        /// <summary>
        /// 连接配置异常信息
        /// </summary>
        public const string CONNECTION_CONFIG = "CONNECTION_CONFIG";
        
        /// <summary>
        /// 参与过审批操作信息
        /// </summary>
        public const string SQL_ACTOR_RECORD = "SQL_ACTOR_RECORD";

        /// <summary>
        /// 获取工作流实例
        /// </summary>
        public const string SQL_WORKFLOW_INSTANCE = "SQL_WORKFLOW_INSTANCE";

        /// <summary>
        /// 审批过程记录
        /// </summary>
        public const string SQL_WORKFLOW_PROCESS = "SQL_WORKFLOW_PROCESS";

        

        private static readonly ResourceManager resourceManage =new ResourceManager("Smartflow.SmartflowResource",Assembly.GetExecutingAssembly());
        
        public static string GetString(string key)
        {
            return resourceManage.GetString(key);
        }
    }
}
