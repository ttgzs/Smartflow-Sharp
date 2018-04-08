using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface IQuery<T>
    {
        List<T> QueryWorkflowNode(long nodeID);
    }
}
