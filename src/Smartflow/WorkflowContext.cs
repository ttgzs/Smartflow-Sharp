/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using Smartflow.Enums;

namespace Smartflow
{
    public class WorkflowContext
    {
        public WorkflowInstance Instance
        {
            get;
            set;
        }

        public long ActorID
        {
            get;
            set;
        }

        public string ActorName
        {
            get;
            set;
        }

        public dynamic Data
        {
            get;
            set;
        }

        public string TransitionID
        {
            get;
            set;
        }

        public WorkflowAction Operation
        {
            get;
            private set;
        }

        internal void SetOperation(WorkflowAction operation)
        {
            this.Operation = operation;
        }
    }
}
