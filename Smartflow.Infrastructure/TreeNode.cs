using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Infrastructure
{
    public class TreeNode : IEntry
    {
        public string EID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string ParentCode
        {
            get;
            set;
        }

        public List<TreeNode> Children
        {
            get;
            set;
        }
    }
}
