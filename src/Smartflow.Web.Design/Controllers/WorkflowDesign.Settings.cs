using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Smartflow.Integration;

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

        public JsonResult GetUserList(int pageIndex,int pageSize,string searchKey)
        {
            return Json(new
            {
                rows= designService.GetUserList(searchKey),
                records = 4
            });

        }
    }
}
