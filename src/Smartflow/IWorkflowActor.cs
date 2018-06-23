using System;
namespace Smartflow
{
    public interface IWorkflowActor
    {
        System.Data.DataTable GetRecord(string instanceID);
    }
}
