/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;

namespace Smartflow.Infrastructure
{
    /// <summary>
    /// 定义基础数据的载体接口
    /// </summary>
    public interface IEntry
    {
        /// <summary>
        /// 标识
        /// </summary>
        long ID
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        string Name
        {
            get;
            set;
        }
    }
}
