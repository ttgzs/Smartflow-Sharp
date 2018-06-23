/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 定义统一日志处理
    /// </summary>
    public class WorkflowLoggingService : ILogging
    {
        private EventLog logging = new EventLog();

        public WorkflowLoggingService()
        {
            logging.Source = ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME);
        }

        public virtual void Error(string exception)
        {
            logging.WriteEntry(exception, EventLogEntryType.Error);
        }

        public virtual void Info(string message)
        {
            logging.WriteEntry(message, EventLogEntryType.Information);
        }
    }
}
