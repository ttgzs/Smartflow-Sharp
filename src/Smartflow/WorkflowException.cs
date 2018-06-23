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
            logging.Error(innerException);
        }

        public WorkflowException(Exception innerException, string instanceID)
            : base(ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME), innerException)
        {
            logging.Error(innerException);
        }

        public WorkflowException(string message)
            : base(message)
        {
            logging.Info(message);
        }
    }
}
