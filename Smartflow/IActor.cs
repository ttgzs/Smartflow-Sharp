/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen
 Email:237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Elements;

namespace Smartflow
{
    public interface IActor
    {
        List<WorkflowActor> GetActors();

        List<WorkflowActor> GetNextActors(long ID);

        bool CheckActor(long actorID);
    }
}
