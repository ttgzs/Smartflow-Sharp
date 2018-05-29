/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.DesignService.Models;
using Smartflow.DesignService;
using System.Web.Script.Serialization;
using Smartflow.Infrastructure;

namespace Smartflow.Web.Design.Controllers
{
    public partial class WorkflowDesignController : Controller
    {
        private WorkflowDesignService designService = new WorkflowDesignService();

        public ActionResult Design(string id)
        {
            ViewBag.WFID = id;
            return View();
        }

        public JsonResult GetWorkflowXml(string WFID)
        {
            WorkflowXml model = designService.GetWorkflowXml(WFID);
            return Json(model);
        }

        public JsonResult Save(WorkflowXml model)
        {
            if (String.IsNullOrEmpty(model.WFID))
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

        public ActionResult WorkflowImage(string instanceID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewBag.Result = serializer.Serialize(WorkflowInstance.GetInstance(instanceID));
            return View();
        }

        public ActionResult WorkflowDesignSettings()
        {
            return View();
        }

        public JsonResult GetRole(string roleIds)
        {
            return Json(designService.GetRole(roleIds));
        }

        public JsonResult GetConfigs()
        {
            return Json(designService.GetConfigs());
        }
    }
}
