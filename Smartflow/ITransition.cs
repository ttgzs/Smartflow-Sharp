/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow
{
    /// <summary>
    /// 路线接口
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// 依据流程实例获取路线
        /// </summary>
        /// <returns>路线实例</returns>
        Transition GetTransition();
    }
}
