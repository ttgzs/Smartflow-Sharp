/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 定义统一的异常处理
    /// </summary>
    public class WorkflowException:Exception
    {
        public WorkflowException() : base() { }

        public WorkflowException(string message) : base(message) { }

    }
}
