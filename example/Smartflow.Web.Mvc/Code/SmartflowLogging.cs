using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartflow.Web.Mvc.Code
{
    public class SmartflowLogging:WorkflowLoggingService
    {
        public override void Error(Exception ex)
        {
            base.Error(ex);
        }

        public override void Info(string message)
        {
            base.Info("cust"+message);
        }
    }
}