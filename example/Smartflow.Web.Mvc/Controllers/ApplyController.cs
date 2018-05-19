using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Smartflow.BussinessService.Models;
using Smartflow.BussinessService;
using Smartflow.Infrastructure;

namespace Smartflow.Web.Controllers
{
    public class ApplyController : Controller
    {
        private BaseWorkflowService bwf = BaseWorkflowService.Instance;

        private ApplyService aservice = new ApplyService();

        public ActionResult Save(Apply model)
        {
            if (model.AID == 0)
            {
                aservice.Persistent(model);

            }
            else
            {
                aservice.Update(model);
            }

            return RedirectToAction("ApplyList");
        }

        public ActionResult SubmitApply(Apply model)
        {
            model.INSTANCEID = bwf.Start(model.WFID);
            if (model.AID == 0)
            {
                aservice.Persistent(model);
            }
            else
            {
                aservice.Update(model);
            }
            return RedirectToAction("ApplyList");
        }

        public ActionResult ApplyList()
        {
            return View(aservice.Query());
        }

        public void Delete(long id)
        {
            aservice.Delete(id);
        }

        public ActionResult Apply(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.AID = 0;
                GenerateDropDownViewData("");
                GenerateDropDownSecretViewData("");
                return View();
            }
            else
            {
                ViewBag.AID = id;
                Apply apply = aservice.GetInstance(long.Parse(id));

                if (apply.STATE == 1)
                {
                    ViewBag.ButtonName = bwf.GetCurrentNodeName(apply.INSTANCEID);
                }
                else
                {
                    ViewBag.ButtonName = "审核";
                }
                GenerateDropDownSecretViewData(apply.SECRETGRADE);
                GenerateDropDownViewData(apply.WFID);
                return View(apply);
            }
        }

        public void GenerateDropDownViewData(string WFID)
        {
            List<WorkflowXml> workflowXmlList = WorkflowXmlService.GetWorkflowXmlList();
            List<SelectListItem> fileList = new List<SelectListItem>();
            foreach (WorkflowXml item in workflowXmlList)
            {
                fileList.Add(new SelectListItem { Text = item.NAME, Value = item.WFID, Selected = (item.WFID == WFID) });
            }
            ViewData["Wfile"] = fileList;
        }


        public void GenerateDropDownSecretViewData(string secretGrade)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "非密", Value = "非密", Selected = ("非密" == secretGrade) });
            list.Add(new SelectListItem { Text = "秘密", Value = "秘密", Selected = ("秘密" == secretGrade) });
            list.Add(new SelectListItem { Text = "机密", Value = "机密", Selected = ("机密" == secretGrade) });
            list.Add(new SelectListItem { Text = "绝密", Value = "绝密", Selected = ("绝密" == secretGrade) });

            ViewData["SC"] = list;
        }
    }
}
