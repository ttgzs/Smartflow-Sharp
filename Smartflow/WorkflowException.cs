using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowException:Exception
    {
        public WorkflowException() : base() { }

        public WorkflowException(string message) : base(message) { }

    }
}
