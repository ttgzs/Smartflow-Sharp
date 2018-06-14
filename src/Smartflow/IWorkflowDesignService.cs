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
