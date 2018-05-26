using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class User
    {
        public long ID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string UserPwd
        {
            get;
            set;
        }

        public string EmployeeName
        {
            get;
            set;
        }

        public string OrgCode
        {
            get;
            set;
        }

        public string OrgName
        {
            get;
            set;
        }
    }
}
