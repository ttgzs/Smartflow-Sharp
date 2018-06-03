/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Smartflow.Elements;
using Smartflow.Enums;

namespace Smartflow.BussinessService.WorkflowService
{
    public class BaseWorkflowEngine : WorkflowEngine
    {
        private readonly static BaseWorkflowEngine singleton = new BaseWorkflowEngine();

        protected BaseWorkflowEngine()
            : base()
        {
        }

        public static WorkflowEngine CreateWorkflowEngine()
        {
            return singleton;
        }

        protected override bool CheckAuthorization(WorkflowContext context)
        {
            WorkflowInstance instance = context.Instance;
            if (instance.Current.NodeType == WorkflowNodeCategeory.Decision)
            {
                return true;
            }
            else
            {
                bool result = true;
                //跳转节点
                if (context.Operation == WorkflowAction.Jump)
                {
                    //获取所有参与组织（多个角色）
                    List<Group> list = instance.Current.Groups;
                    //依据多个参与组织验证当前审批人是否有审批权限
                    //如是没有审批权限，返回false,否则返回true
                }
                else if (context.Operation == WorkflowAction.Rollback)
                {
                    //流程回退

                }
                else if (context.Operation == WorkflowAction.Undo)
                {
                    //流程撤销
                }
                return result;
            }
        }
    }
}
