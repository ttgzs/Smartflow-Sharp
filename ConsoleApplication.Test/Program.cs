using Smartflow;
using Smartflow.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConsoleApplication.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            //首次启动工作流程
            //WorkflowEngine engine = WorkflowEngine.CreateWorkflowEngine("1");
            WorkflowEngine engine = WorkflowEngineExt.CreateWorkflowEngine();
            engine.OnProcess = (exectionContext) =>
             {

                 Console.WriteLine(exectionContext.Data.Name);

                 Console.WriteLine(string.Format("instanceID:{0} From:{1} To:{2}", exectionContext.Instance.InstanceID, exectionContext.From.NAME, exectionContext.To.NAME));
             };

            engine.OnCompleted = (exectionContext) =>
            {
                Console.WriteLine("流程结束");
            };

            WorkflowInstance instance = engine.GetWorkflowInstance("8accf23a-48d7-4e9f-8e3a-ed05fb102fe4");
            ASTNode currentNode = instance.Current;

            if (instance.Current.NodeType == Smartflow.Enums.WorkflowNodeCategeory.End)
            {
                Console.WriteLine("流程结束");
            }
            else
            {
                Transition tran = currentNode.Transitions.First();
                engine.Jump(instance, tran.NID, tran.TO, data:new { Name="程德忍" });

               // engine.Jump(instance, tran.NID, instance.Current.PreviousTransition.FROM);
            }*/
            Console.ReadKey();
        }
    }
}
