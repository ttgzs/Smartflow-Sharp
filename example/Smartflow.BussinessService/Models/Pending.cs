using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class Pending
    {
        public long IDENTIFICATION
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

        public string APPELLATION
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
