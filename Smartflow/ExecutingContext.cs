/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen
 Email:237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Elements;

namespace Smartflow
{
    /// <summary>
    /// 工作流执行下文参数封装
    /// </summary>
    public class ExecutingContext
    {
        /// <summary>
        /// 当前节点
        /// </summary>
        public ASTNode From
        {
            get;
            set;
        }

        /// <summary>
        /// 跳转到节点
        /// </summary>
        public ASTNode To
        {
            get;
            set;
        }

        /// <summary>
        /// 跳转路线ID
        /// </summary>
        public string TID
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流实例
        /// </summary>
        public WorkflowInstance Instance
        {
            get;
            set;
        }

        /// <summary>
        /// 传递数据
        /// </summary>
        public dynamic Data
        {
            get;
            set;
        }

        /// <summary>
        /// 设置下一节点审批人
        /// </summary>
        /// <param name="wfActors"></param>
        public void SetActor(List<WorkflowActor> wfActors)
        {
            List<Actor> actors = new List<Actor>();
            foreach (WorkflowActor wfActor in wfActors)
            {
                actors.Add(new Actor()
                {
                    NAME = wfActor.Name,
                    ID = wfActor.ID
                });
            }

            To.SetActor(actors);
        }
    }
}
