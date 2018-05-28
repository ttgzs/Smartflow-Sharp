using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class Pending
    {
        public long ID
        {
            get;
            set;
        }

        public long ACTORID
        {
            get;
            set;
        }
        

        public string NODEID
        {
            get;
            set;
        }

        public string INSTANCEID
        {
            get;
            set;
        }

        public string NAME
        {
            get;
            set;
        }

        public string ACTION
        {
            get;
            set;
        }
    }
}
