using Smartflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication.Test
{
    class WorkflowEngineExt : WorkflowEngine
    {

        protected override bool CheckAuthorization(WorkflowInstance instance, long actorID)
        {

            return true;

        }

        public static WorkflowEngine CreateWorkflowEngine()
        {
            return new WorkflowEngineExt();
        }
    }
}
