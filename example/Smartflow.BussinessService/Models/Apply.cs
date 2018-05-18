using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartflow.BussinessService.Models
{
    public class Apply
    {
        public long AID
        {
            get;
            set;
        }

        public int STATE
        {
            get;
            set;
        }

        public string NAME
        {
            get;
            set;
        }

        public string DESCRIPTION
        {
            get;
            set;
        }

        public string WFID
        {
            get;
            set;
        }

        public string INSTANCEID
        {
            get;
            set;
        }

        public string APPLYLEVEL
        {
            get;
            set;
        }

        public string STATENAME
        {
            get
            {
                string state = "待提交";
                switch (STATE)
                {
                    case 0:
                        break;
                    case 1:
                        state = "审核中";
                        break;
                    case 8:
                        state = "已完成";
                        break;
                    default:
                        break;
                }
                return state;
            }
        }
    }
}