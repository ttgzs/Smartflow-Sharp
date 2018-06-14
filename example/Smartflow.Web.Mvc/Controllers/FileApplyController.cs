using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.BussinessService.Models;
using Smartflow.BussinessService.WorkflowService;
using Smartflow.BussinessService.Services;
using Smartflow.Web.Mvc.Code;

namespace Smartflow.Web.Controllers
{
    public class FileApplyController : Controller
    {
        private BaseWorkflowService bwfs = BaseWorkflowService.Instance;
        private ApplyService fileApplyService = new ApplyService();

        public ActionResult Save(Apply model)
        {
            model.STATUS = 0;
            fileApplyService.Persistent(model);
            return RedirectToAction("FileApplyList");
        }

        public ActionResult Submit(Apply model)
        {
            model.INSTANCEID = bwfs.Start(model.STRUCTUREID);
            model.STATUS = 1;
            fileApplyService.Persistent(model);
            return RedirectToAction("FileApplyList");
        }

        public ActionResult FileApplyList()
        {
            return View(fileApplyService.Query());
        }

        public void Delete(long id)
        {
            fileApplyService.Delete(id);
        }

        public ActionResult FileApply(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                GenerateSecretViewData(string.Empty);
                GenerateWFViewData(string.Empty);
                return View();
            }
            else
            {
                Apply mdl = fileApplyService.GetInstance(long.Parse(id));
                GenerateSecretViewData(mdl.SECRETGRADE);
                GenerateWFViewData(mdl.STRUCTUREID);

                if (mdl.STATUS == 1)
                {
                    var executeNode = bwfs.GetCurrentPrevNode(mdl.INSTANCEID);
                    var current = bwfs.GetCurrent(mdl.INSTANCEID);

                    ViewBag.ButtonName = current.APPELLATION;
                    ViewBag.PreviousButtonName = executeNode == null ? String.Empty : executeNode.APPELLATION;
                    ViewBag.UndoCheck = CommonMethods.CheckUndoButton(mdl.INSTANCEID);
                    ViewBag.UndoAuth = executeNode == null ? true : CommonMethods.CheckUndoAuth(mdl.INSTANCEID);
                    ViewBag.JumpAuth = current.APPELLATION == "开始" ? true : CommonMethods.CheckAuth(current.NID, mdl.INSTANCEID);
                    ViewBag.UserList= new UserService().GetPendingUserList(current.NID, mdl.INSTANCEID);
                }
                return View(mdl);
            }
        }

        public void GenerateWFViewData(string WFID)
        {
            List<WorkflowStructure> workflowXmlList = WorkflowStructureService.GetWorkflowStructureList();
            List<SelectListItem> fileList = new List<SelectListItem>();
            foreach (WorkflowStructure item in workflowXmlList)
            {
                fileList.Add(new SelectListItem { Text = item.APPELLATION, Value = item.IDENTIFICATION, Selected = (item.IDENTIFICATION == WFID) });
            }
            ViewData["WFiles"] = fileList;
        }

        public void GenerateSecretViewData(string secretGrade)
        {
            List<string> secrets = new List<string>() { 
              "非密",
              "秘密",
              "机密",
              "绝密"
            };
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (string secret in secrets)
            {
                list.Add(new SelectListItem { Text = secret, Value = secret, Selected = (secret == secretGrade) });
            }
            ViewData["SECRET"] = list;
        }
    }
}
