using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class ExecutingContext
    {
        public ASTNode From
        {
            get;
            set;
        }

        public ASTNode To
        {
            get;
            set;
        }

        public long TID
        {
            get;
            set;
        }

        public WorkflowInstance Instance
        {
            get;
            set;
        }
    }
}
