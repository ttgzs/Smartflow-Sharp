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
    /// <summary>
    /// 定义外键接口
    /// </summary>
    public interface IRelationShip
    {
        string RNID
        {
            get;
            set;
        }
    }
}
