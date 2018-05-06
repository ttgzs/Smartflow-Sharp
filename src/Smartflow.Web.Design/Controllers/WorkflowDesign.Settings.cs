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

        public JsonResult GetUserList(int draw,int length,int start,string searchKey)
        {
            return Json(new
            {
                draw = 1,
                recordsTotal = 4,
                recordsFiltered = 4,
                data = designService.GetUserList(searchKey)

            });

        }
    }
}
