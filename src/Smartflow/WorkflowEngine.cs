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
        private IWorkflow workflowService = WorkflowFactoryProvider.OfType<IFactory>().CreateWorkflowSerivce();

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
        /// 进行流程跳转
        /// </summary>
        /// <param name="instance">工作流实例</param>
        /// <param name="transitionID">路线NID</param>
        /// <param name="transitionTo">跳转节点ID</param>
        /// <param name="actorID">审批人ID</param>
        /// <param name="data">附加数据</param>
        public void Jump(WorkflowInstance instance, string transitionID, long transitionTo, long actorID = 0,dynamic data = null)
        {
            if (instance.State == WorkflowInstanceState.Running)
            {
                WorkflowNode current = instance.Current;

                if (CheckAuthorization(instance, actorID) == false) return;
                
                //记录已经参与审批过的人信息
                current.SetActor(actorID);

                instance.Jump(transitionTo);

                ASTNode to = current.GetNode(transitionTo);

                OnExecuteProcess(new ExecutingContext()
                {
                    From = current,
                    To = to,
                    TID = transitionID,
                    Instance = instance,
                    Data = data
                });
             
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
                    Jump(WorkflowInstance.GetInstance(instance.InstanceID), transitionID, tran.DESTINATION, actorID, data);
                }
            }
        }

        /// <summary>
        /// 流程回退、撤销
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="actorID"></param>
        /// <param name="data"></param>
        public void Return(WorkflowInstance instance,long actorID = 0, dynamic data = null)
        {
            if (instance.State == WorkflowInstanceState.Running)
            {
                WorkflowNode current = instance.Current.GetPreviousNode();

                if (CheckAuthorization(instance, actorID) == false) return;

                //记录已经参与审批过的人信息
                current.SetActor(actorID);

                instance.Jump(current.ID);

                ASTNode to = current.GetNode(current.ID);

                OnExecuteProcess(new ExecutingContext()
                {
                    From = current,
                    To = to,
                    TID = instance.Current.Previous.NID,
                    Instance = instance,
                    Data = data
                });

                if (to.NodeType == WorkflowNodeCategeory.Decision)
                {
                    WorkflowDecision wfDecision = WorkflowDecision.GetNodeInstance(to);
                    Transition tran = wfDecision.GetTransition();
                    if (tran == null) return;
                    Return(WorkflowInstance.GetInstance(instance.InstanceID), actorID, data);
                }
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
