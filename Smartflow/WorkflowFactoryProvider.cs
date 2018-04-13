/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen
 Email:237552006@qq.com
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public sealed  class WorkflowFactoryProvider
    {
        private WorkflowFactoryProvider()
        {

        }

        private static IList<object> _collection = new List<object>() 
        { 
            new WorkflowServiceFactory()
        };

        public static IList<object> Collection
        {
            get { return _collection; }
        }

        public static T OfType<T>()
        {
            return (T)Collection.Where(o => (o is T)).FirstOrDefault();
        }
    }
}
