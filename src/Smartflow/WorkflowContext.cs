using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowContext
    {
        private WorkflowInstance instance = null;
        private String transitionID = string.Empty;
        private long actorID = 0;
        private dynamic data = null;
        private WorkflowAction action = WorkflowAction.Jump;

        public WorkflowInstance Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public string TransitionID
        {
            get { return transitionID; }
            set { transitionID = value; }
        }

        public long To
        {
            get;
            set;
        }

        public long ActorID
        {
            get { return actorID; }
            set { actorID = value; }
        }

        public dynamic Data
        {
            get { return data; }
            set { data = value; }
        }

        public WorkflowAction Action
        {
            get { return action; }
            set { action = value; }
        }


        public WorkflowNode Current
        {
            get
            {
                return (Action == WorkflowAction.Undo) ?
                        instance.Current.GetFromNode() :
                        instance.Current;
            }
        }
    }
}
