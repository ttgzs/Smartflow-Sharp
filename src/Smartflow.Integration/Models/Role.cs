using System;
using System.Collections.Generic;

namespace Smartflow.Integration.Models
{
    public class Role:IEntry
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
    }
}