using Smartflow.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index(string id)
        {
            ViewBag.WfID = (string.IsNullOrEmpty(id))?"0":id;
            return View();
        }


        public ActionResult Main()
        {
            return View();
        }


        public JsonResult GetResult(string wfID)
        {
            return Json(WorkflowUtils.GetWorkflowXml(wfID));
        } 


        public ActionResult List()
        {
            return View(WorkflowUtils.GetWorkflowXmlList());
        }

        public JsonResult Save(Smartflow.Web.Code.WorkflowXml model)
        {
            if (String.IsNullOrEmpty(model.WFID))
            {
                model.WFID = Guid.NewGuid().ToString();
                WorkflowUtils.Persistent(model);
            }
            else
            {
                WorkflowUtils.ModWorkflowXml(model);
            }
            return Json(true);
        }
    }
}
