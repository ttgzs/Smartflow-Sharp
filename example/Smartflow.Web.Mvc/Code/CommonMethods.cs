using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Smartflow.BussinessService.Models;
using Smartflow.BussinessService.Services;
using Smartflow.BussinessService.WorkflowService;

namespace Smartflow.Web.Mvc.Code
{
    public class CommonMethods
    {
        public static bool CheckAuth(string nodeID, string instanceID)
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            return new PendingService().Check(userInfo.ID.ToString(), nodeID, instanceID);
        }

        public static bool CheckUndoAuth(string instanceID)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            return instance.Current.GetFromNode().GetActors().Count(e => e.ID == userInfo.ID) > 0;
        }

        public static bool CheckUndoButton(string instanceID)
        {
            string currentNodeName = BaseWorkflowService.Instance.GetCurrent(instanceID).NAME;
            var executeNode = BaseWorkflowService.Instance.GetCurrentPrevNode(instanceID);
            return (currentNodeName == "开始" || currentNodeName == "结束" || executeNode.NAME == "开始");
        }
    }
}