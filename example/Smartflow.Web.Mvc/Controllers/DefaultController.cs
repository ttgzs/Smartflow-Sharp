using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Services;

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

        public JsonResult GetUser(string userName)
        {
            //演示使用
            Smartflow.BussinessService.Models.User userInfo = new UserService().GetUser(userName);

            if (userInfo == null)
            {
                return Json(false);
            }
            else
            {
                System.Web.HttpContext.Current.Session["user"] = userInfo;
                return Json(true);
            }
        }
    }
}
