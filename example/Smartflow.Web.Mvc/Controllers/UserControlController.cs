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
using System.Dynamic;
using Smartflow;
using Smartflow.BussinessService;

namespace Smartflow.Web.Controllers
{
    public class UserControlController : Controller
    {
        private RecordService workflowRecordService = new RecordService();
        private BaseWorkflowService bwkf = BaseWorkflowService.Instance;

        public PartialViewResult Record(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            return PartialView(workflowRecordService.Query(instanceID));
        }

        public ActionResult WorkflowCheck(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            return View(instance.Current.Transitions);
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="instanceID"></param>
        /// <returns></returns>
        public JsonResult UndoSubmit(string instanceID)
        {
            bwkf.UndoSubmit(instanceID);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="transitionID"></param>
        /// <param name="to"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public JsonResult Jump(string instanceID, string transitionID, long to, string message, string action)
        {
            dynamic dynData = new ExpandoObject();
            dynData.Message = message;
            //请不要直接定义匿名类传递
            if (action == "rollback")
            {
                bwkf.Rollback(instanceID, 0, dynData);
            }
            else
            {
                bwkf.Jump(instanceID, transitionID, to, data: dynData);
            }
            return Json(true);
        }
    }
}
