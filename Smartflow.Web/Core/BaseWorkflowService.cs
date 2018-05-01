/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Smartflow.Web.Code;

namespace Smartflow.Web.Core
{
    public sealed class BaseWorkflowService
    {
        private static WorkflowEngine context = WorkflowEngine.CreateWorkflowEngine();
        private readonly static BaseWorkflowService singleton = new BaseWorkflowService();

        private BaseWorkflowService()
        {
            //关闭授权验证（默认关闭）
            context.EnableValidation = false;

            WorkflowEngine.OnProcess += new DelegatingProcessHandle(OnProcess);
            WorkflowEngine.OnCompleted += new DelegatingCompletedHandle(OnCompleted);
        }

        public static BaseWorkflowService Instance
        {
            get { return singleton; }
        }

        private RecordService recordService = new RecordService();

        public void OnCompleted(ExecutingContext executeContext)
        {
            //流程结束（在完成事件中可以做业务操作）



        }

        public void OnProcess(ExecutingContext executeContext)
        {
            recordService.Persistent(new Record()
            {
                INSTANCEID = executeContext.Instance.InstanceID,
                NODENAME = executeContext.From.NAME,
                MESSAGE = executeContext.Data.Message

            });
        }

        public string GetCurrentNodeName(string WFID)
        {
            return context.GetWorkflowInstance(WFID)
                .Current.NAME;
        }

        public string Start(string WFID)
        {
            return context.Start(WFID);
        }

        public WorkflowInstance GetInstance(string instanceID)
        {
            return context.GetWorkflowInstance(instanceID);
        }

        public void Jump(string instanceID, string transitionID, long transitionTo, long actorID = 0, dynamic data = null)
        {
            WorkflowInstance instance = context.GetWorkflowInstance(instanceID);
            context.Jump(instance, transitionID, transitionTo, actorID, data);
        }
    }
}