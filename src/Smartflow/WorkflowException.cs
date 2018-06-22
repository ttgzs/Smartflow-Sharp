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
        private ILogging logging = WorkflowServiceProvider.OfType<ILogging>();

        public WorkflowException(Exception innerException)
            : base(ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME), innerException)
        {
            logging.Write(innerException.ToString());
        }

        public WorkflowException(Exception innerException, string instanceID)
            : base(ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME), innerException)
        {
            logging.Write(string.Format("流程实例ID:{0} 异常信息 {1} ", instanceID, innerException));
        }

        public WorkflowException(string message)
            : base(message)
        {
            logging.Write(message);
        }
    }
}
