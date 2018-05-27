/**
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
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

        public WorkflowAction Action
        {
            get;
            private set;
        }

        internal void SetAction(WorkflowAction action)
        {
            this.Action = action;
        }
    }
}
