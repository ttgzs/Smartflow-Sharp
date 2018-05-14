/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.Integration;


namespace Smartflow.Web.Controllers
{
    public class UserControlController : Controller
    {
        private WorkflowRecordService workflowRecordService = new WorkflowRecordService();
        private BaseWorkflowService bwkf = BaseWorkflowService.Instance;

        public PartialViewResult Record(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            return PartialView(workflowRecordService.Query(instanceID));
        }

        public JsonResult GetWorkflowImage(string instanceID)
        {
            return Json(bwkf.GetInstance(instanceID));
        }

        public JsonResult Jump(string instanceID, string transitionID, long to, string message)
        {
            bwkf.Jump(instanceID, transitionID, to, data: new { Message = message });
            return Json(true);
        }
    }
}
