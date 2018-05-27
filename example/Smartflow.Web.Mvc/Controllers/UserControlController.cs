/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using Smartflow;
using Smartflow.BussinessService;
using Smartflow.Elements;
using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Models;

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

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="transitionID"></param>
        /// <returns></returns>
        public JsonResult GetUsers(string instanceID, string transitionID)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            List<Group> groupList = instance.Current.GetNextGroup(transitionID);
            List<string> gList = new List<string>();
            foreach (Group g in groupList)
            {
                gList.Add(g.ID.ToString());
            }

            List<User> userList = new UserService()
                      .GetUserList(string.Join(",", gList));
            return Json(userList);
        }

        public ActionResult WorkflowCheck(string instanceID)
        {
            ViewBag.InstanceID = instanceID;
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            return View(instance.Current.GetTransitions());
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
        /// <param name="message"></param>
        /// <returns></returns>
        public JsonResult Jump(string instanceID, string transitionID, string message, string action)
        { 
            //请不要直接定义匿名类传递
            dynamic dynData = new ExpandoObject();
            dynData.Message = message;
           
            if (action == "rollback")
            {
                bwkf.Rollback(instanceID, 0, dynData);
            }
            else
            {
                bwkf.Jump(instanceID, transitionID, actorID: 0, data: dynData);
            }
            return Json(true);
        }
    }
}
