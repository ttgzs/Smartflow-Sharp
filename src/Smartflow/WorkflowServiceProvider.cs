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
    public static class WorkflowServiceProvider
    {
        private static IList<object> _collection = new List<object>();

        static WorkflowServiceProvider()
        {
            _collection.Add(new WorkflowLoggingService());
            _collection.Add(new WorkflowService());
            _collection.Add(new MailService());
            _collection.Add(new WorkflowConfig());
            _collection.Add(new WorkflowDesignService());
        }




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
