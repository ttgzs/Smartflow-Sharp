/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
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

        public JsonResult GetWorkflowStructure(string WFID)
        {
            WorkflowStructure model = designService.GetWorkflowStructure(WFID);
            return Json(model);
        }

        public JsonResult Save(WorkflowStructure model)
        {
            if (String.IsNullOrEmpty(model.IDENTIFICATION))
            {
                model.IDENTIFICATION = Guid.NewGuid().ToString();
                model.FILESTRUCTURE = HttpUtility.UrlDecode(model.FILESTRUCTURE);
                designService.Persistent(model);
            }
            else
            {
                model.FILESTRUCTURE = HttpUtility.UrlDecode(model.FILESTRUCTURE);
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
