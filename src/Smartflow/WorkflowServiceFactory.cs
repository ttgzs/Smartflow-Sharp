/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class WorkflowServiceFactory:IFactory
    {
        public  IWorkflow CreateWorkflowSerivce()
        {
            return new WorkflowService();
        }
    }
}
