using Smartflow.Web.Code;
using Smartflow.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class UserControlController : Controller
    {
        private RecordService recordService = new RecordService();

        public PartialViewResult Record(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            return PartialView(recordService.Query(instanceID));
        }

        public ActionResult WorkflowImage(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            return View();
        }

        public ActionResult WorkflowCheck(string instanceID)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            WorkflowInstance instance = bwkf.GetInstance(instanceID);
            ViewBag.InstanceID = instanceID;
            return View(instance.Current.Transitions);
        }

        public JsonResult GetWorkflowImage(string instanceID)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            return Json(bwkf.GetInstance(instanceID));
        }

        public JsonResult Jump(string instanceID, string transitionID, long to, string message)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            bwkf.Jump(instanceID, transitionID, to, data: new { Message = message });
            return Json(true);
        }
    }
}
