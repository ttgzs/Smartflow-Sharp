using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface IActor
    {
        List<WorkflowActor> GetActors();

        List<WorkflowActor> GetNextActors(string ID);

        bool CheckActor(long actorID);
    }
}
