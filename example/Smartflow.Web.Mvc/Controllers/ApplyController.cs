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
                ViewBag.UndoCheck = 0;
                ViewBag.AID = 0;
                GenerateWFViewData("");
                GenerateViewData("");
                return View();
            }
            else
            {
                ViewBag.AID = id;
                Apply apply = applyService.GetInstance(long.Parse(id));

                if (apply.STATUS == 1)
                {
                    ViewBag.ButtonName = bwf.GetCurrentNodeName(apply.INSTANCEID);
                    ViewBag.PreviousButtonName = bwf.GetCurrentPrevNodeName(apply.INSTANCEID);
                    ViewBag.UndoCheck = CheckUndoButton(apply.INSTANCEID) ? 0 : 1;
                }
                else
                {
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

        public bool CheckUndoButton(string instanceID)
        {
            string currentNodeName = bwf.GetCurrentNodeName(instanceID);
            return (currentNodeName == "开始" || currentNodeName == "结束" || bwf.GetCurrentPrevNodeName(instanceID) == "开始");
        }
    }
}
