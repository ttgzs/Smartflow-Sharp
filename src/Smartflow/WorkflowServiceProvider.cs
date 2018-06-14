/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public sealed  class WorkflowServiceProvider
    {
        private WorkflowServiceProvider()
        {
        }

        private static IList<object> _collection = new List<object>() 
        { 
            new WorkflowService(),
            new WorkflowDesignService()
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
