using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.Infrastructure;
using Smartflow.BussinessService.Models;
using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Services;

namespace Smartflow.Web.Controllers
{
    public class ApplyController : Controller
    {
        private BaseWorkflowService bwf = BaseWorkflowService.Instance;
        private ApplyService applyService = new ApplyService();

        public ActionResult Save(Apply model)
        {
            applyService.Persistent(model);
            return RedirectToAction("ApplyList");
        }

        public ActionResult SubmitApply(Apply model)
        {
            model.INSTANCEID = bwf.Start(model.WFID);
            applyService.Persistent(model);
            return RedirectToAction("ApplyList");
        }

        public ActionResult ApplyList()
        {
            return View(applyService.Query());
        }

        public void Delete(long id)
        {
            applyService.Delete(id);
        }

        public ActionResult Apply(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.UndoAuth = true;
                ViewBag.JumpAuth = true;
                ViewBag.UndoCheck = 0;
                GenerateWFViewData("");
                GenerateViewData("");
                return View();
            }
            else
            {
                Apply apply = applyService.GetInstance(long.Parse(id));
                if (apply.STATUS == 1)
                {
                    var prevExecuteNode = bwf.GetCurrentPrevNode(apply.INSTANCEID);
                    var current=bwf.GetCurrent(apply.INSTANCEID);
                    ViewBag.ButtonName = current.NAME;
                    ViewBag.PreviousButtonName =prevExecuteNode==null?String.Empty:prevExecuteNode.NAME;
                    ViewBag.UndoCheck = CheckUndoButton(apply.INSTANCEID) ? 0 : 1;
                    ViewBag.UndoAuth = prevExecuteNode == null ? true : 
                                      CheckUndoAuth(WorkflowInstance.GetInstance(apply.INSTANCEID));

                    ViewBag.JumpAuth =current.NAME=="开始"?true: CheckAuth(current.NID, apply.INSTANCEID);
                }
                else
                {
                    ViewBag.UndoAuth = true;
                    ViewBag.JumpAuth = true;
                    ViewBag.ButtonName = "审核";
                    ViewBag.PreviousButtonName = "撤销";
                    ViewBag.UndoCheck = 0;
                }
                GenerateViewData(apply.SECRETGRADE);
                GenerateWFViewData(apply.WFID);
                return View(apply);
            }
        }

        public void GenerateWFViewData(string WFID)
        {
            List<WorkflowXml> workflowXmlList = WorkflowXmlService.GetWorkflowXmlList();
            List<SelectListItem> fileList = new List<SelectListItem>();
            foreach (WorkflowXml item in workflowXmlList)
            {
                fileList.Add(new SelectListItem { Text = item.NAME, Value = item.WFID, Selected = (item.WFID == WFID) });
            }
            ViewData["WFiles"] = fileList;
        }

        public void GenerateViewData(string secretGrade)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "非密", Value = "非密", Selected = ("非密" == secretGrade) });
            list.Add(new SelectListItem { Text = "秘密", Value = "秘密", Selected = ("秘密" == secretGrade) });
            list.Add(new SelectListItem { Text = "机密", Value = "机密", Selected = ("机密" == secretGrade) });
            list.Add(new SelectListItem { Text = "绝密", Value = "绝密", Selected = ("绝密" == secretGrade) });
            ViewData["SC"] = list;
        }


        public bool CheckAuth(string nodeID, string instanceID)
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            return new PendingService().Check(userInfo.ID.ToString(), nodeID, instanceID);
        }

        public bool CheckUndoAuth(WorkflowInstance instance)
        {
            User userInfo = System.Web.HttpContext.Current.Session["user"] as User;
            return instance.Current.GetFromNode().GetActors().Count(e=>e.ID==userInfo.ID)>0;
        }

        public bool CheckUndoButton(string instanceID)
        {
            string currentNodeName = bwf.GetCurrent(instanceID).NAME;
            var prevExecuteNode = bwf.GetCurrentPrevNode(instanceID);
            return (currentNodeName == "开始" || currentNodeName == "结束" || prevExecuteNode.NAME == "开始");
        }
    }
}
