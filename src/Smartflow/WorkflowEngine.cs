/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Elements;
using Smartflow.Enums;

namespace Smartflow
{
    /// <summary>
    /// 工作流引擎，由工作流引擎统一提供对外服务接口，以此来驱动流程跳转
    /// </summary>
    public abstract class WorkflowEngine
    {
        private IWorkflow workflowService = WorkflowServiceProvider.OfType<IWorkflow>();

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
        protected abstract bool CheckAuthorization(WorkflowContext context);

        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="workflowStructure">文件</param>
        /// <returns>返回实例ID</returns>
        public string Start(string identification)
        {
            IWorkflowDesignService service = WorkflowServiceProvider.OfType<IWorkflowDesignService>();
            WorkflowStructure workflowStructure = service.GetWorkflowStructure(identification);
            return workflowService.Start(workflowStructure);
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
        /// <param name="context"></param>
        public void Jump(WorkflowContext context)
        {
            WorkflowInstance instance = context.Instance;
            if (instance.State == WorkflowInstanceState.Running)
            {
                WorkflowNode current = instance.Current;

                context.SetOperation(WorkflowAction.Jump);

                if (CheckAuthorization(context) == false) return;

                long transitionTo = current.Transitions
                                  .FirstOrDefault(e => e.NID == context.TransitionID).DESTINATION;

                current.SetActor(context.ActorID, context.ActorName, WorkflowAction.Jump);
                instance.Jump(transitionTo);

                ASTNode to = current.GetNode(transitionTo);
                OnExecuteProcess(new ExecutingContext()
                {
                    From = current,
                    To = to,
                    TransitionID = context.TransitionID,
                    Instance = instance,
                    Data = context.Data,
                    Operation = context.Operation,
                    ActorID = context.ActorID,
                    ActorName = context.ActorName
                });

                if (to.NodeType == WorkflowNodeCategeory.End)
                {
                    instance.State = WorkflowInstanceState.End;
                    instance.Transfer();
                }
                else if (to.NodeType == WorkflowNodeCategeory.Decision)
                {
                    WorkflowDecision wfDecision = WorkflowDecision.ConvertToReallyType(to);
                    Transition transition = wfDecision.GetTransition();
                    if (transition == null) return;
                    Jump(new WorkflowContext()
                    {
                        Instance = WorkflowInstance.GetInstance(instance.InstanceID),
                        TransitionID = transition.NID,
                        ActorID = context.ActorID,
                        Data = context.Data
                    });
                }
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="context"></param>
        public void Cancel(WorkflowContext context)
        {
            WorkflowInstance instance = context.Instance;
            if (instance.State == WorkflowInstanceState.Running)
            {
                WorkflowNode current = instance.Current.GetFromNode();

                context.SetOperation(WorkflowAction.Undo);
                if (CheckAuthorization(context) == false) return;

                //记录已经参与审批过的人信息
                current.SetActor(context.ActorID, context.ActorName, WorkflowAction.Undo);

                instance.Jump(current.IDENTIFICATION);

                ASTNode to = current.GetNode(current.IDENTIFICATION);

                OnExecuteProcess(new ExecutingContext()
                {
                    From = current,
                    To = to,
                    TransitionID = instance.Current.FromTransition.NID,
                    Instance = instance,
                    Data = context.Data,
                    Operation = context.Operation,
                    ActorID = context.ActorID,
                    ActorName = context.ActorName
                });

                if (to.NodeType == WorkflowNodeCategeory.Decision)
                {
                    WorkflowNode wfDecision = WorkflowNode.ConvertToReallyType(to);
                    Transition transition = wfDecision.FromTransition;

                    if (transition == null) return;

                    Cancel(new WorkflowContext()
                    {
                        Instance = WorkflowInstance.GetInstance(instance.InstanceID),
                        ActorID = context.ActorID,
                        Data = context.Data
                    });
                }
            }
        }

        /// <summary>
        /// 流程回退、
        /// </summary>
        /// <param name="context"></param>
        public void Rollback(WorkflowContext context)
        {
            WorkflowInstance instance = context.Instance;
            if (instance.State == WorkflowInstanceState.Running)
            {
                WorkflowNode current = instance.Current.GetFromNode();
                context.SetOperation(WorkflowAction.Rollback);
                if (CheckAuthorization(context) == false) return;

                //记录已经参与审批过的人信息
                current.SetActor(context.ActorID, context.ActorName, WorkflowAction.Rollback);

                instance.Jump(current.IDENTIFICATION);

                ASTNode to = current.GetNode(current.IDENTIFICATION);

                OnExecuteProcess(new ExecutingContext()
                {
                    From = instance.Current,
                    To = to,
                    TransitionID = instance.Current.FromTransition.NID,
                    Instance = instance,
                    Data = context.Data,
                    Operation = context.Operation,
                    ActorID = context.ActorID,
                    ActorName = context.ActorName
                });

                if (to.NodeType == WorkflowNodeCategeory.Decision)
                {
                    WorkflowNode wfDecision = WorkflowNode.ConvertToReallyType(to);
                    Transition transition = wfDecision.FromTransition;

                    if (transition == null) return;

                    Rollback(new WorkflowContext()
                    {
                        Instance = WorkflowInstance.GetInstance(instance.InstanceID),
                        ActorID = context.ActorID,
                        Data = context.Data
                    });
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
                ORIGIN = executeContext.From.IDENTIFICATION,
                DESTINATION = executeContext.To.IDENTIFICATION,
                TRANSITIONID = executeContext.TransitionID,
                INSTANCEID = executeContext.Instance.InstanceID,
                NODETYPE = executeContext.From.NodeType,
                OPERATION = executeContext.Operation
            });
        }
    }
}
