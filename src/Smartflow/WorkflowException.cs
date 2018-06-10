/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowException : ApplicationException
    {
        public WorkflowException(Exception innerException)
            : base("Smartflow-Sharp", innerException)
        {
            WorkflowLogger.WriteLog(innerException.ToString());
        }

        public WorkflowException(Exception innerException, string instanceID)
            : base("Smartflow-Sharp", innerException)
        {
            WorkflowLogger.WriteLog(string.Format("流程实例ID:{0} 异常信息 {1} ", instanceID, innerException));
        }

        public WorkflowException(string message)
            : base(message)
        {
            WorkflowLogger.WriteLog(message);
        }
    }
}
