using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.Integration;
using Smartflow.Integration.Models;

namespace Smartflow.Web.Design.Controllers
{
    public partial class WorkflowDesignController
    {
        public ActionResult WorkflowDesignSettings()
        {
            return View();
        }

        public JsonResult GetRole(string roleIds)
        {
            return Json(designService.GetRole(roleIds));
        }

        public JsonResult GetOrgTree()
        {
            return Json(designService.GetOrganization());
        }

        public JsonResult GetConfigs()
        {
            return Json(designService.GetConfigs());
        }

        public JsonResult GetUserList(int page, int rows, string code, string searchKey, string userIdStr)
        {
            Dictionary<string, object> queryArg = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(code) && "000" != code)
            {
                queryArg.Add("Code", code);
            }
            if (!String.IsNullOrEmpty(searchKey))
            {
                queryArg.Add("SearchKey", searchKey);
            }

            if (!String.IsNullOrEmpty(userIdStr))
            {
                queryArg.Add("UserIds", userIdStr);
            }

            int total;
            IList<IEntry> userList = designService.GetUserList(page, rows, out total, queryArg);
            return Json(new
            {
                rows = userList,
                records = total
            });
        }
    }
}
