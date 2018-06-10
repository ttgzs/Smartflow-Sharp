/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
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
        long Identification
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        string Appellation
        {
            get;
            set;
        }
    }
}
