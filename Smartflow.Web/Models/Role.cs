using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Smartflow.Integration;

namespace Smartflow.Web.Models
{
    public class Role:IEntry
    {
        public long EID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}