using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Infrastructure;
namespace Smartflow.DesignService.Models
{
    public class Config : IEntry
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

        public string Connecte
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
