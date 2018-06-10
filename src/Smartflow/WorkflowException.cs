using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowException:ApplicationException
    {
        public WorkflowException(Exception innerException)
            : base("Smartflow-Sharp", innerException)
        {
            WorkflowLogger.WriteLog(innerException.ToString());
        }

        public WorkflowException()
            : base()
        {

        }

        public WorkflowException(string message)
            : base(message)
        {
            WorkflowLogger.WriteLog(message);
        }
    }
}
