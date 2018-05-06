/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;

namespace Smartflow.Integration
{
    public class TreeNode : IEntry
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

        public string Code
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
