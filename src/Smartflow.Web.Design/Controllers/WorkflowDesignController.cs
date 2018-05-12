/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.Integration.Models;
using Smartflow.Integration;

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
           WorkflowXml model= designService.GetWorkflowXml(WFID);
           return Json(model);
        }

        public ActionResult List()
        {
            return View(designService.GetWorkflowXmlList());
        }

        public JsonResult Save(WorkflowXml model)
        {
            if ("0" == model.WFID || String.IsNullOrEmpty(model.WFID))
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

        public ActionResult WorkflowImage(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            return View();
        }

        public JsonResult GetWorkflowImage(string instanceID)
        {
            return Json(BaseWorkflowService.Instance.GetInstance(instanceID));
        }
    }
}
