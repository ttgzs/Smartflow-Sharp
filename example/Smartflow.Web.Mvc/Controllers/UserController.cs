using Smartflow.BussinessService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smartflow.Web.Mvc.Controllers
{
    public class UserController : Controller
    {
        private UserService userService = new UserService();

        //
        // GET: /User/
        public ActionResult UserList()
        {
            return View(userService.GetUserList());
        }

        //
        // GET: /User/
        public ActionResult RoleStatistics()
        {
            return View(userService.GetStatisticsDataTable());
        }
    }
}
