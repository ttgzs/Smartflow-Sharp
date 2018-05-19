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

using Smartflow.Elements;
using Smartflow.Enums;
using Smartflow.Infrastructure;

namespace Smartflow
{
    /// <summary>
    /// 工作流引擎，由工作流引擎统一提供对外服务接口，以此来驱动流程跳转
    /// </summary>
    public abstract class WorkflowEngine
    {
        protected IWorkflow workflowService = WorkflowFactoryProvider.OfType<IFactory>()
            .CreateWorkflowSerivce();


        public static event DelegatingProcessHandle OnProcess;

        public static event DelegatingCompletedHandle OnCompleted;

 

        /// <summary>
        /// 触发流程跳转事件
        /// </summary>
        /// <param name="executeContext">执行上下文</param>
        protected void OnExecuteProcess(ExecutingContext executeContext)
        {
            Processing(executeContext);
            OnProcess(executeContext);
            if (OnCompleted != null && executeContext.To.NodeType == WorkflowNodeCategeory.End)
            {
                OnCompleted(executeContext);
            }
        }

        /// <summary>
        /// 检查是否授权
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="actorID">审批人</param>
        /// <returns>true：授权 false：未授权</returns>
        protected abstract bool CheckAuthorization(WorkflowInstance instance, long actorID);

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="flowID">文件ID</param>
        /// <returns>返回实例ID</returns>
        public string Start(WorkflowXml workflowXml)
        {
            return workflowService.Start(workflowXml);
        }

        /// <summary>
        /// 终止流程
        /// </summary>
        /// <param name="instance">工作流实例</param>
        public void Kill(WorkflowInstance instance)
        {
            workflowService.Kill(instance);
        }

        /// <summary>
        /// 中断流程
        /// </summary>
        /// <param name="instance">工作流实例</param>
        public void Terminate(WorkflowInstance instance)
        {
            workflowService.Terminate(instance);
        }

        /// <summary>
        /// 恢复流程
        /// </summary>
        /// <param name="instance">工作流实例</param>
        public void Revert(WorkflowInstance instance)
        {
            workflowService.Revert(instance);
        }

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="instanceID">实例ID</param>
        /// <returns>工作流程实例</returns>
        public WorkflowInstance GetWorkflowInstance(string instanceID)
        {
            return workflowService.Instance(instanceID);
        }

        /// <summary>
        /// 进行流程跳转（流程跳转对外提供统一的接口服务，该接口服务包括跳转、回退、撤销）
        /// </summary>
        /// <param name="instance">工作流实例</param>
        /// <param name="transitionID">路线NID</param>
        /// <param name="transitionTo">跳转节点ID</param>
        /// <param name="actorID">审批人ID</param>
        /// <param name="data">附加数据</param>
        public void Jump(WorkflowInstance instance, string transitionID, long transitionTo, long actorID = 0, dynamic data = null)
        {
            if (instance.State == WorkflowInstanceState.Running)
            {
                ASTNode currentNode = instance.Current;

                if (CheckAuthorization(instance, actorID) == false) return;

                instance.Jump(transitionTo);
                ASTNode to = instance.Current.GetNode(transitionTo);

                if (to.NodeType == WorkflowNodeCategeory.End)
                {
                    instance.State = WorkflowInstanceState.End;
                    instance.Transfer();
                }
                else if (to.NodeType == WorkflowNodeCategeory.Decision)
                {
                    WorkflowDecision wfDecision = WorkflowDecision.GetNodeInstance(to);
                    Transition tran = wfDecision.GetTransition();

                    if (tran == null) return;
                    Jump(instance, transitionID, tran.DESTINATION, actorID, data);
                }

                OnExecuteProcess(new ExecutingContext()
                {
                    From = instance.Current,
                    To = to,
                    TID = transitionID,
                    Instance = instance,
                    Data = data
                });
            }
        }

        /// <summary>
        /// 跳转过程处理入库
        /// </summary>
        /// <param name="executeContext">执行上下文</param>
        protected void Processing(ExecutingContext executeContext)
        {
            workflowService.Processing(new WorkflowProcess()
            {
                RNID = executeContext.To.NID,
                SOURCE = executeContext.From.ID,
                DESTINATION = executeContext.To.ID,
                TID = executeContext.TID.ToString(),
                INSTANCEID = executeContext.Instance.InstanceID,
                NODETYPE = executeContext.From.NodeType
            });
        }
    }
}
