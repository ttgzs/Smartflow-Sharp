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

            //首次启动工作流程
            //WorkflowEngine engine = WorkflowEngine.CreateWorkflowEngine("1");
            WorkflowEngine engine = WorkflowEngineExt.CreateWorkflowEngine();
             engine.OnProcess = (exectionContext) =>
              {

                  Console.WriteLine(exectionContext.Data.Name);

                  Console.WriteLine(string.Format("instanceID:{0} From:{1} To:{2}",
                      exectionContext.Instance.InstanceID,
                      exectionContext.From.NAME,
                      exectionContext.To.NAME));
              };

             engine.OnCompleted = (exectionContext) =>
             {

                 Console.WriteLine(string.Format("instanceID:{0} From:{1} To:{2}",
                     exectionContext.Instance.InstanceID,
                     exectionContext.From.NAME,
                     exectionContext.To.NAME));

                 Console.WriteLine("流程结束");
             };

             WorkflowInstance instance = engine.GetWorkflowInstance("2253aa14-2570-4e66-9236-b1ac4a43af3d");
             ASTNode currentNode = instance.Current;


             if (instance.Current.NodeType == Smartflow.Enums.WorkflowNodeCategeory.End)
             {
                 //Console.WriteLine("流程结束");
             }
             else
             {
                  //Transition tran = currentNode.Transitions.First();
                 
                 
                  Transition tran = instance.Current.Previous;
                  engine.Jump(instance, tran.NID, tran.FROM, data: new { Name = "程德忍" });
             }
            Console.ReadKey();
        }
    }
}
