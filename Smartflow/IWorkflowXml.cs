using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface IWorkflowXml
    {
        string WFID
        {
            get;
            set;
        }

        string XML
        {
            get;
            set;
        }

        string IMAGE
        {
            get;
            set;
        }
    }
}
