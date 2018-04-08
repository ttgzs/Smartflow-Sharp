using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface IWorkflow
    {
        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="workflowXml"></param>
        /// <returns></returns>
        string Start(WorkflowXml workflowXml);

        /// <summary>
        /// 获取工作流实例
        /// </summary>
        /// <param name="instanceID">实例ID</param>
        /// <returns>工作流实例</returns>
        WorkflowInstance Instance(string instanceID);

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
        /// 获取工作流程
        /// </summary>
        /// <param name="flowID">流程版本ID</param>
        /// <returns>工作流</returns>
        WorkflowXml GetWorkflowXml(string flowID);

        /// <summary>
        /// 获取节点实例
        /// </summary>
        /// <param name="instanceID">流程实例ID</param>
        /// <param name="ID">节点ID</param>
        /// <returns>节点实例</returns>
        ASTNode GetNode(string instanceID, string ID);


        void Processing(IPersistent persistent);
    }
}
