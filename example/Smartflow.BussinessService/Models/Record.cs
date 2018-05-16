using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartflow.BussinessService.Models
{
    public class Record
    {
        public long ID
        {
            get;
            set;
        }

        public string NODENAME
        {
            get;
            set;
        }

        public string MESSAGE
        {
            get;
            set;
        }

        public string INSTANCEID
        {
            get;
            set;
        }
    }
}