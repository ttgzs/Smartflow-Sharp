using Smartflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication.Test
{
    public class WorkflowEngineExt : WorkflowEngine
    {
        protected override bool CheckAuthorization(WorkflowInstance instance, long actorID)
        {
            return true;
        }

        public new static WorkflowEngine CreateWorkflowEngine()
        {
            return new WorkflowEngineExt();
        }
    }
}
