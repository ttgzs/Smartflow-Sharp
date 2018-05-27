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

        //审批记录服务
        private RecordService recordService = new RecordService();

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
            model.STATE = 8;
            applyService.Update(model);
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
                //写待办业务
            }
        }

        public string GetCurrentNodeName(string WFID)
        {
            return WorkflowInstance.GetInstance(WFID).Current.NAME;
        }

        public string GetCurrentPrevNodeName(string WFID)
        {
            var current = WorkflowInstance.GetInstance(WFID).Current;
            var preNode = current.GetFromNode();
            return preNode == null ? "" : preNode.NAME;
        }

        public void UndoSubmit(string WFID, long actorID = 0)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(WFID);
            dynamic dynData = new ExpandoObject();
            dynData.Message = "撤销此节点";
            context.Cancel(new WorkflowContext()
            {
                Instance = instance,
                Data = dynData,
                ActorID = actorID
            });
        }

        public void Rollback(string WFID, long actorID = 0, dynamic dynData = null)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(WFID);
            context.Rollback(new WorkflowContext()
            {
                Instance = instance,
                Data = dynData,
                ActorID = actorID
            });
        }

        public List<Group> GetCurrentActorGroup(string WFID)
        {
            return WorkflowInstance.GetInstance(WFID).Current.Groups;
        }

        public string Start(string WFID)
        {
            WorkflowXml wfXml = WorkflowXmlService.GetWorkflowXml(WFID);
            return context.Start(wfXml);
        }

        public void Jump(string instanceID, string transitionID, long actorID = 0, dynamic data = null)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            context.Jump(new WorkflowContext()
            {
                Instance = instance,
                TransitionID = transitionID,
                Data = data,
                ActorID = actorID,
            });
        }
    }
}