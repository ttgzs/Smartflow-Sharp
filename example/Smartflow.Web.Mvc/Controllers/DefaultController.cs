using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smartflow.BussinessService;

namespace Smartflow.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(WorkflowXmlService.GetWorkflowXmlList());
        }

        public JsonResult Delete(string WFID)
        {
            WorkflowXmlService.Delete(WFID);
            return Json(true);
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}
