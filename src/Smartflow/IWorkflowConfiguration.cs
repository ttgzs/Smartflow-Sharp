using System;
using System.Data;


namespace Smartflow
{
    public interface IWorkflowConfiguration
    {
        DataTable GetWorkflowConfigs();
    }
}
