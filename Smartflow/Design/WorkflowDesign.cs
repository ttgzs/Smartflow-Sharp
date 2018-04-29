using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    public class WorkflowDesign
    {
        protected  IWorkflowDesignService Context = new WorkflowDesignService();

        public WorkflowDesign()
        {

        }

        public WorkflowDesign(IWorkflowDesignService context)
        {
            this.Context = context;
        }





    }
}
