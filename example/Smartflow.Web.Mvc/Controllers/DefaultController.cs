using Smartflow.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class DefaultController : Controller
    {
        private WorkflowDesignService designService = new WorkflowDesignService();
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(designService.GetWorkflowXmlList());
        }

        public JsonResult Delete(string WFID)
        {
            designService.Delete(WFID);
            return Json(true);
        }
    }
}
