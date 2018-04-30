using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartflow.Web.Code
{
    public class BaseWorkflowService
    {
        private static WorkflowEngine context = WorkflowEngineExt.CreateWorkflowEngine();
        private static BaseWorkflowService singleton = new BaseWorkflowService();


        private BaseWorkflowService()
        {
            WorkflowEngine.OnProcess += new DelegatingProcessHandle(OnProcess);
            WorkflowEngine.OnCompleted += new DelegatingCompletedHandle(OnCompleted);
        }

        public static BaseWorkflowService Instance
        {
            get
            {
                return singleton;
            }
        }

        private RecordService rsservice = new RecordService();
     


        public void OnCompleted(ExecutingContext executeContext)
        {



        }

        public void OnProcess(ExecutingContext executeContext)
        {
            rsservice.Persistent(new Record()
            {
                INSTANCEID = executeContext.Instance.InstanceID,
                NODENAME = executeContext.From.NAME,
                MESSAGE=executeContext.Data.Message
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