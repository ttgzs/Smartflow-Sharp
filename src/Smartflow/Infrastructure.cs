using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class Infrastructure
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
        }

        protected ILogging LogService
        {
            get { return WorkflowServiceProvider.OfType<ILogging>(); }
        }
    }
}
