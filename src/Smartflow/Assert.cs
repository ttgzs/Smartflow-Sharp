/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class Assert
    {
        public static void CheckNull(object o, string paramName)
        {
            if (o==null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void StringNull(string checkStr,string paramName)
        {
            if (String.IsNullOrEmpty(checkStr))
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
