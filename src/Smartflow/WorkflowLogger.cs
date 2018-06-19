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
    public class WorkflowLogger
    {
        public static void WriteLog(Exception ex)
        {
            WorkflowLogger.WriteLog(ex.ToString());
        }

        public static void WriteLog(string message)
        {
            EventLog logger = new EventLog();

            logger.Source = ResourceManage.GetString(ResourceManage.SMARTFLOW_SHARP_NAME);
            logger.WriteEntry(message, EventLogEntryType.Error);
        }
    }
}
