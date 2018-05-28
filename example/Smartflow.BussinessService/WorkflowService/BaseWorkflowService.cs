/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.BussinessService.Models;
using Smartflow.Infrastructure;
using Smartflow.Elements;
using System.Dynamic;
using Smartflow.BussinessService.Services;

namespace Smartflow.BussinessService.WorkflowService
{
    public sealed class BaseWorkflowService
    {
        private static WorkflowEngine context = BaseWorkflowEngine.CreateWorkflowEngine();
        private readonly static BaseWorkflowService singleton = new BaseWorkflowService();
        private RecordService recordService = new RecordService();
        private PendingService pendingService = new PendingService();

        private BaseWorkflowService()
        {
            WorkflowEngine.OnProcess += new DelegatingProcessHandle(OnProcess);
            WorkflowEngine.OnCompleted += new DelegatingCompletedHandle(OnCompleted);
        }

        public static BaseWorkflowService Instance
        {
            get { return singleton; }
        }

        public void OnCompleted(ExecutingContext executeContext)
        {
            //流程结束（在完成事件中可以做业务操作）
            ApplyService applyService = new ApplyService();
            Apply model = applyService.GetInstanceByInstanceID(executeContext.Instance.InstanceID);
            model.STATUS = 8;
            applyService.Persistent(model);
        }

        public void OnProcess(ExecutingContext executeContext)
        {
            if (executeContext.Instance.Current.NodeType != Enums.WorkflowNodeCategeory.Decision)
            {
                var dny = executeContext.Data;
                recordService.Persistent(new Record()
                {
                    INSTANCEID = executeContext.Instance.InstanceID,
                    NODENAME = executeContext.From.NAME,
                    MESSAGE = executeContext.Data.Message
                });

                if (executeContext.Action == Enums.WorkflowAction.Jump)
                {
                    //写待办业务
                    foreach (var item in dny.Actors)
                    {
                        new PendingService().Persistent(new Pending()
                        {
                            ACTORID=item.ID,
                            ACTION=executeContext.Action.ToString(),
                            INSTANCEID = executeContext.Instance.InstanceID,
                            NODEID = GetCurrentNode(executeContext.Instance.InstanceID).NID,
                            NAME="<a href='#'>你有待办任务。</a>"
                        });
                    }
                }
            }
        }

        public string GetCurrentNodeName(string instanceID)
        {
            return GetCurrentNode(instanceID).NAME;
        }

        public string GetCurrentPrevNodeName(string instanceID)
        {
            var current = GetCurrentNode(instanceID);
            var node = current.GetFromNode();
            return (node == null ? string.Empty : node.NAME);
        }

        private WorkflowNode GetCurrentNode(string instanceID)
        {
            return WorkflowInstance.GetInstance(instanceID).Current;
        }

        public void UndoSubmit(string instanceID, long actorID = 0)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            dynamic dynData = new ExpandoObject();
            dynData.Message = "撤销此节点";
            context.Cancel(new WorkflowContext()
            {
                Instance = instance,
                Data = dynData,
                ActorID = actorID
            });
        }

        public void Rollback(string instanceID, long actorID, string actorName, dynamic data)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            context.Rollback(new WorkflowContext()
            {
                Instance = instance,
                Data = data,
                ActorID = actorID,
                ActorName = actorName
            });
        }

        public List<Group> GetCurrentActorGroup(string instanceID)
        {
            return WorkflowInstance.GetInstance(instanceID).Current.Groups;
        }

        public string Start(string WFID)
        {
            WorkflowXml wfXml = WorkflowXmlService.GetWorkflowXml(WFID);
            return context.Start(wfXml);
        }

        public void Jump(string instanceID, string transitionID, long actorID, string actorName, dynamic data)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            context.Jump(new WorkflowContext()
            {
                Instance = instance,
                TransitionID = transitionID,
                Data = data,
                ActorID = actorID,
                ActorName = actorName
            });
        }
    }
}