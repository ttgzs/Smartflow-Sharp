/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 */
using System;
using System.Collections.Generic;
using Smartflow.Infrastructure;

namespace Smartflow.DesignService.Models
{
    public class Role:IEntry
    {
        public long Identification
        {
            get;
            set;
        }

        public string Appellation
        {
            get;
            set;
        }
    }
}