using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface ITransition
    {
        Transition GetTransition(string instanceID);
    }
}
