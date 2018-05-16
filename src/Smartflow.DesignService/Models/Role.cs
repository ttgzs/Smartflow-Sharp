using System;
using System.Collections.Generic;
using Smartflow.Infrastructure;

namespace Smartflow.DesignService.Models
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