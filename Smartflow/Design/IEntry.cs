using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Design
{
    /// <summary>
    /// 定义基础数据的载体接口
    /// </summary>
    public interface IEntry
    {
        /// <summary>
        /// 标识
        /// </summary>
        string EID
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
