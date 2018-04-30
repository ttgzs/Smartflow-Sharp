using Smartflow.Design;
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
        private WorkflowDesign designService = new WorkflowDesign();

        public ActionResult Index(string id)
        {
            ViewBag.WfID = (string.IsNullOrEmpty(id)) ? "0" : id;
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public JsonResult GetResult(string WFID)
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
