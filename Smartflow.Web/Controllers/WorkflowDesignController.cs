using Smartflow.Design;
using Smartflow.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class WorkflowDesignController : Controller
    {
        private WorkflowDesign designService = new WorkflowDesign(new WorkflowInfrastructure());

        public ActionResult Index(string id)
        {
            ViewBag.WFID = id;
            return View();
        }

        public JsonResult GetWorkflowXml(string WFID)
        {
            return Json(designService.GetWorkflowXml(WFID));
        }

        public ActionResult List()
        {
            return View(designService.GetWorkflowXmlList());
        }

        public JsonResult Save(WorkflowXml model)
        {
            if ("0" == model.WFID)
            {
                model.WFID = Guid.NewGuid().ToString();
                model.XML = HttpUtility.UrlDecode(model.XML);
                designService.Persistent(model);
            }
            else
            {
                model.XML = HttpUtility.UrlDecode(model.XML);
                designService.Update(model);
            }
            return Json(true);
        }

        public JsonResult Delete(string WFID)
        {
            designService.Delete(WFID);
            return Json(true);
        }
    }
}
