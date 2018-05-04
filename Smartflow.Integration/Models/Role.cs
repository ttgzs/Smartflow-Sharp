using System;
using System.Collections.Generic;

namespace Smartflow.Integration.Models
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