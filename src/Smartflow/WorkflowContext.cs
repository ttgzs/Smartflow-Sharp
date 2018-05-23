using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowContext : BaseContext
    {
        public string TransitionID
        {
            get;
            set;
        }

        public long To
        {
            get;
            set;
        }
    }
}
