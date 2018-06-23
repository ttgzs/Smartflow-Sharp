/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Data;

namespace Smartflow
{
    public interface IWorkflowActor
    {
        DataTable GetRecord(string instanceID);
    }
}
