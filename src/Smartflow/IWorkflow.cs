/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow;

namespace Smartflow
{
    /// <summary>
    /// 定义工作流服务接口
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="workflowStructure"></param>
        /// <returns></returns>
        string Start(WorkflowStructure workflowStructure);

        /// <summary>
        /// 终结流程
        /// </summary>
        /// <param name="instance">流程实例</param>
        void Kill(WorkflowInstance instance);

        /// <summary>
        /// 中断流程
        /// </summary>
        /// <param name="instance">流程实例</param>
        void Terminate(WorkflowInstance instance);

        /// <summary>
        /// 恢复流程
        /// </summary>
        /// <param name="instance">流程实例</param>
        void Revert(WorkflowInstance instance);

        /// <summary>
        /// 监控跳转过程
        /// </summary>
        /// <param name="persistent">持久化接口</param>
        void Processing(IPersistent persistent);
    }
}
