using Smartflow.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class UserCtlController : Controller
    {
        private RecordService rservice = new RecordService();


        //
        // GET: /UserCtl/
        public PartialViewResult Record(string instanceID)
        {

            ViewBag.InstanceID = instanceID;

            return PartialView(rservice.Query(instanceID));
        }

        public ActionResult FlowImage(string instanceID)
        {
            ViewBag.WfID = instanceID;
            return View();
        }


        public JsonResult GetResult(string instanceID)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            return Json(bwkf.GetInstance(instanceID));
        }

        //
        // GET: /UserCtl/
        public ActionResult FlowControl(string instanceID)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            WorkflowInstance instance = bwkf.GetInstance(instanceID);

            ViewBag.InstanceID = instanceID;

            return View(instance.Current.Transitions);
        }


        public JsonResult Jump(string instanceID, string transitionID, long to, string message)
        {
            BaseWorkflowService bwkf = BaseWorkflowService.Instance;
            bwkf.Jump(instanceID, transitionID, to, data: new { Message = message });
            return Json(true);
        }
    }
}
