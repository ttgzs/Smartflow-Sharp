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

using System.Web.Script.Serialization;
using Smartflow;
using System.Data;

namespace Smartflow.Web.Design.Controllers
{
    public partial class WorkflowDesignController : BaseController
    {
        private IWorkflowDesignService designService = WorkflowServiceProvider.OfType<IWorkflowDesignService>();
        private ActorService roleService = new ActorService();

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

            DataTable dt = WorkflowActor.GetRecord(instanceID);
            ViewBag.Record = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            return View();
        }

        public ActionResult WorkflowDesignSettings()
        {
            return View();
        }

        public JsonResult GetRole(string roleIds, string searchKey)
        {
            return JsonWrapper(roleService.GetRole(roleIds, searchKey));
        }

        public JsonResult GetConfigs()
        {
            return JsonWrapper(WorkflowServiceProvider
                .OfType<IWorkflowConfiguration>()
                .GetWorkflowConfigs());
        }
    }
}
