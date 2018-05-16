using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult Delete(string WFID)
        {
           // designService.Delete(WFID);
            return Json(true);
        }
    }
}
