/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.Enums
{
    public enum WorkflowAction
    {
        /// <summary>
        /// 流程流转
        /// </summary>
        Jump,
        /// <summary>
        /// 流程撤销
        /// </summary>
        Undo,
        /// <summary>
        /// 流程退回
        /// </summary>
        Rollback
    }
}
