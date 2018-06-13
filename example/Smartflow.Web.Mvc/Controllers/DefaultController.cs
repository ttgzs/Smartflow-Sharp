using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Services;
using Smartflow.BussinessService.Models;

namespace Smartflow.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Main()
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            ViewBag.EmployeeName = userInfo.EMPLOYEENAME;
            return View();
        }

        public ActionResult List()
        {
            return View(WorkflowStructureService.GetWorkflowStructureList());
        }

        public JsonResult Delete(string WFID)
        {
            WorkflowStructureService.Delete(WFID);
            return Json(true);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Pending()
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            return View(new PendingService().Query(userInfo.IDENTIFICATION));
        }

        public JsonResult GetPendingCount()
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            if (userInfo == null)
            {
                return Json(0);
            }
            else
            {
                return Json(new PendingService().Query(userInfo.IDENTIFICATION).Count);
            }
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
