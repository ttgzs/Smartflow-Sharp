using Smartflow.Design;
using Smartflow.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class FormController : Controller
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


        //
        // GET: /Form/
        public ActionResult Apply(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.AID = 0;
                GenerateDropDownViewData("");
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
                GenerateDropDownViewData(apply.WFID);
                return View(apply);
            }
        }

        public void GenerateDropDownViewData(string WFID)
        {
            WorkflowDesign designService = new WorkflowDesign();
            List<WorkflowXml> workflowXmlList = designService.GetWorkflowXmlList();
            List<SelectListItem> fileList = new List<SelectListItem>();
            foreach (WorkflowXml item in workflowXmlList)
            {
                fileList.Add(new SelectListItem { Text = item.NAME, Value = item.WFID, Selected = (item.WFID == WFID) });
            }
            ViewData["Wfile"] = fileList;
        }
    }
}
