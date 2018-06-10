using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartflow.BussinessService.Models
{
    public class Apply
    {
        public long IDENTIFICATION
        {
            get;
            set;
        }

        public int STATUS
        {
            get;
            set;
        }

        public string FNAME
        {
            get;
            set;
        }

        public string DESCRIPTION
        {
            get;
            set;
        }
        
        public string STRUCTUREID
        {
            get;
            set;
        }

        public string INSTANCEID
        {
            get;
            set;
        }

        public string SECRETGRADE
        {
            get;
            set;
        }

        public string STATUSNAME
        {
            get
            {
                string state = "待提交";
                switch (STATUS)
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