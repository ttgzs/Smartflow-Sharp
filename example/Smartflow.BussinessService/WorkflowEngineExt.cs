using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService
{
    public class WorkflowEngineExt : WorkflowEngine
    {
        private readonly static WorkflowEngineExt singleton = new WorkflowEngineExt();

        protected WorkflowEngineExt()
            : base()
        {

        }

        public static WorkflowEngine CreateWorkflowEngine()
        {
            return singleton;
        }

        protected override bool CheckAuthorization(WorkflowInstance instance, long actorID)
        {
            return true;
        }
    }
}
