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
            logging.Error(innerException.ToString());
        }

        public WorkflowException(Exception innerException, string instanceID)
            : base(ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME), innerException)
        {
            logging.Error(string.Format("异常消息：{0} 实例ID：{1}", innerException.ToString(), instanceID));
        }

        public WorkflowException(string message)
            : base(message)
        {
            logging.Error(message);
        }
    }
}
