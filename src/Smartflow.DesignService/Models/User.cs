using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smartflow.Infrastructure;

namespace Smartflow.DesignService.Models
{
    public class User : IEntry
    {
        public long ID
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return EmployeeName;
            }
            set
            {



            }
        }

        public string UserName
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

        public string EmployeeName
        {
            get;
            set;
        }
    }
}
