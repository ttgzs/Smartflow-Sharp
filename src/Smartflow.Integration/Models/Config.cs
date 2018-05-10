using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Integration.Models
{
    public class Config : IEntry
    {
        public long ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }


        public string Connection
        {
            get;
            set;
        }

        public string DbCategory
        {
            get;
            set;
        }
    }
}
