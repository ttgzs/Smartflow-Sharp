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
using Smartflow.Elements;


using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Models;
using Smartflow.BussinessService.Services;

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
        /// 依据跳转跳线获取下节点审批用户列表
        /// </summary>
        /// <param name="instanceID">流程实例</param>
        /// <param name="transitionID">跳转路线ID</param>
        /// <returns>用户列表</returns>
        public JsonResult GetUsers(string instanceID, string transitionID)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            List<Group> groupList = instance.Current.GetNextGroup(transitionID);
            List<string> gList = new List<string>();
            foreach (Group g in groupList)
            {
                gList.Add(g.ID.ToString());
            }
            List<User> userList = new UserService().GetUserList(string.Join(",", gList));

            return Json(userList);
        }

        /// <summary>
        /// 工作流组件
        /// </summary>
        /// <param name="instanceID">流程实例ID</param>
        /// <returns></returns>
        public ActionResult WorkflowCheck(string instanceID, string bussinessID)
        {
            ViewBag.InstanceID = instanceID;
            ViewBag.bussinessID = bussinessID;
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            ViewBag.CheckResult = CheckUndoButton(instanceID);
            return View(instance.Current.GetTransitions());
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        /// <param name="instanceID">流程实例ID</param>
        /// <returns></returns>
        public JsonResult UndoSubmit(string instanceID, string bussinessID)
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            bwkf.UndoSubmit(instanceID, userInfo.ID,bussinessID);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 流程跳转处理接口(请不要直接定义匿名类传递)
        /// </summary>
        /// <param name="instanceID">流程实例ID</param>
        /// <param name="transitionID">跳转路线ID</param>
        /// <param name="message">审批消息</param>
        /// <param name="action">审批动作（原路退回、跳转）</param>
        /// <returns>是否成功</returns>
        public JsonResult Jump(string instanceID, string transitionID, string bussinessID, string message, string action, long[] userIDs, string[] userNames)
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            dynamic data = new ExpandoObject();
            data.Message = message;
            data.bussinessID = bussinessID;
            data.UserInfo = userInfo;
            switch (action.ToLower())
            {
                case "rollback":
                    bwkf.Rollback(instanceID, userInfo.ID, userInfo.EMPLOYEENAME, data);
                    break;
                default:
                    bwkf.Jump(instanceID, transitionID, userInfo.ID, userInfo.EMPLOYEENAME, data);
                    break;
            }
            return Json(true);
        }

        public bool CheckUndoButton(string instanceID)
        {
            string currentNodeName = bwkf.GetCurrentNodeName(instanceID);
            return (currentNodeName == "开始" || currentNodeName == "结束" || bwkf.GetCurrentPrevNodeName(instanceID) == "开始");
        }
    }
}
