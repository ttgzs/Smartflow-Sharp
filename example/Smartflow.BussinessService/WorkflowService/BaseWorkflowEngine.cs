using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService
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

        protected override bool CheckAuthorization(BaseContext context)
        {
            WorkflowInstance instance = context.Instance;
            //获取所有参与组织（多个角色）
            List<Group> list = instance.Current.Groups;
            //依据多个参与组织验证当前审批人是否有审批权限
            //如是没有审批权限，返回false,否则返回true
            return true;
        }
    }
}
