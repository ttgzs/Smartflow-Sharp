/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;

namespace Smartflow
{
   public interface IWorkflowDesignService
    {
        void Delete(string IDENTIFICATION);
        List<WorkflowStructure> GetWorkflowStructureList();
        void Persistent(WorkflowStructure workflowStructure);
        void Update(WorkflowStructure workflowStructure);
        WorkflowStructure GetWorkflowStructure(string identification);

    }
}
