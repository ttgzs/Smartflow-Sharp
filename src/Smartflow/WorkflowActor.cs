/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 工作审批人员
    /// </summary>
    public class WorkflowActor
    {
        /// <summary>
        /// 审批人员ID
        /// </summary>
        public long ID
        {
            get;
            set;
        }

        /// <summary>
        /// 审批人员名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
