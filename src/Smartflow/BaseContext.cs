using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class BaseContext
    {
        private dynamic data = null;

        public WorkflowInstance Instance
        {
            get;
            set;
        }

        public long ActorID
        {
            get;
            set;
        }

        public string ActorName
        {
            get;
            set;
        }

        public dynamic Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
