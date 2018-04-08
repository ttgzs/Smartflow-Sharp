using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public interface IActor
    {
        List<Actor> GetActorList();

        bool CheckActor(long actorID);

        List<Actor> GetActorListByRole();
    }
}
