/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Smartflow.BussinessService.Models;
using Smartflow;
using Smartflow.Elements;
using System.Dynamic;
using Smartflow.BussinessService.Services;

namespace Smartflow.BussinessService.WorkflowService
{
    public sealed class BaseWorkflowService
    {
        private static WorkflowEngine context = BaseWorkflowEngine.CreateWorkflowEngine();
        private readonly static BaseWorkflowService singleton = new BaseWorkflowService();
        private RecordService recordService = new RecordService();
        private PendingService pendingService = new PendingService();

        private BaseWorkflowService()
        {
            WorkflowEngine.OnProcess += new DelegatingProcessHandle(OnProcess);
            WorkflowEngine.OnCompleted += new DelegatingCompletedHandle(OnCompleted);
        }

        public static BaseWorkflowService Instance
        {
            get { return singleton; }
        }

        public void OnCompleted(ExecutingContext executeContext)
        {
            //以下代码仅用于演示
            //流程结束（在完成事件中可以做业务操作）
            ApplyService applyService = new ApplyService();
            Apply model = applyService.GetInstanceByInstanceID(executeContext.Instance.InstanceID);
            model.STATUS = 8;
            applyService.Persistent(model);
            new PendingService().Delete(executeContext.Instance.InstanceID);
        }

        public void OnProcess(ExecutingContext executeContext)
        {
            if (executeContext.Instance.Current.NodeType == Enums.WorkflowNodeCategeory.Decision)
            {
                new PendingService().Delete(executeContext.Instance.Current.NID, executeContext.Instance.InstanceID);
                var current = GetCurrentNode(executeContext.Instance.InstanceID);
                if (executeContext.Operation == Enums.WorkflowAction.Jump&&current.NodeType!=Enums.WorkflowNodeCategeory.Decision)
                {
                    List<User> userList = GetUsersByGroup(current.Groups);
                    //写待办业务
                    foreach (var item in userList)
                    {
                        new PendingService().Persistent(new Pending()
                        {
                            ACTORID = item.IDENTIFICATION,
                            ACTION = executeContext.Operation.ToString(),
                            INSTANCEID = executeContext.Instance.InstanceID,
                            NODEID = GetCurrentNode(executeContext.Instance.InstanceID).NID,
                            APPELLATION = string.Format("<a href=\"javascript:parent.window.document.getElementById('frmContent').src='../FileApply/FileApply/{0}'\">你有待办任务。</a>", executeContext.Data.bussinessID)
                        });
                    }
                    new PendingService().Delete(executeContext.Instance.Current.NID, executeContext.Instance.InstanceID);
                }
            }
            //以下代码仅用于演示
            if (executeContext.Instance.Current.NodeType != Enums.WorkflowNodeCategeory.Decision)
            {
                var dny = executeContext.Data;
                recordService.Persistent(new Record()
                {
                    INSTANCEID = executeContext.Instance.InstanceID,
                    NODENAME = executeContext.From.APPELLATION,
                    MESSAGE = executeContext.Data.Message
                });

                var current = GetCurrentNode(executeContext.Instance.InstanceID);

                if (current.APPELLATION == "结束")
                {
                    new PendingService().Delete(executeContext.Instance.InstanceID);
                }
                else if (executeContext.Operation == Enums.WorkflowAction.Jump)
                {
                    List<User> userList = GetUsersByGroup(current.Groups);
                    //写待办业务
                    foreach (var item in userList)
                    {
                        new PendingService().Persistent(new Pending()
                        {
                            ACTORID = item.IDENTIFICATION,
                            ACTION = executeContext.Operation.ToString(),
                            INSTANCEID = executeContext.Instance.InstanceID,
                            NODEID = GetCurrentNode(executeContext.Instance.InstanceID).NID,
                            APPELLATION = string.Format("<a href=\"javascript:;\" onclick=\"parent.window.document.getElementById('frmContent').src='../FileApply/FileApply/{0}'\">你有待办任务。</a>", dny.bussinessID)
                        });
                    }

                    new PendingService().Delete(executeContext.Instance.Current.NID, executeContext.Instance.InstanceID);
                }
                else if (executeContext.Operation == Enums.WorkflowAction.Rollback)
                {
                    //流程回退(谁审就退给谁) 仅限演示
                    var item = executeContext.Instance.Current
                              .GetFromNode()
                              .GetActors()
                              .FirstOrDefault();

                    new PendingService().Persistent(new Pending()
                    {
                        ACTORID = item.IDENTIFICATION,
                        ACTION = executeContext.Operation.ToString(),
                        INSTANCEID = executeContext.Instance.InstanceID,
                        NODEID = GetCurrentNode(executeContext.Instance.InstanceID).NID,
                        APPELLATION = string.Format("<a href=\"javascript:;\" onclick=\"parent.window.document.getElementById('frmContent').src='../FileApply/FileApply/{0}'\">你有待办任务。</a>", dny.bussinessID)
                    });

                    //WorkflowServiceProvider.OfType<IMailService>().Notification()
                    new PendingService().Delete(executeContext.Instance.Current.NID, executeContext.Instance.InstanceID);
                 }
                else if (executeContext.Operation == Enums.WorkflowAction.Undo)
                {
                    //流程撤销(重新指派人审批) 仅限演示
                    List<Group> items = executeContext.Instance.Current.GetFromNode().Groups;
                    List<User> userList = GetUsersByGroup(items);
                    foreach (User item in userList)
                    {
                        new PendingService().Persistent(new Pending()
                        {
                            ACTORID = item.IDENTIFICATION,
                            ACTION = executeContext.Operation.ToString(),
                            INSTANCEID = executeContext.Instance.InstanceID,
                            NODEID = GetCurrentNode(executeContext.Instance.InstanceID).NID,
                            APPELLATION = string.Format("<a href=\"javascript:;\" onclick=\"parent.window.document.getElementById('frmContent').src='../FileApply/FileApply/{0}'\">你有待办任务。</a>", dny.bussinessID)
                        });
                    }
                    new PendingService().Delete(executeContext.Instance.Current.NID, executeContext.Instance.InstanceID);
                }
            }
        }

        private List<User> GetUsersByGroup(List<Group> items)
        {
            List<string> gList = new List<string>();
            foreach (Group g in items)
            {
                gList.Add(g.IDENTIFICATION.ToString());
            }
            
            if (gList.Count==0)
            {
                return new List<User>();
            }
            
            return new UserService().GetUserList(string.Join(",", gList));
        }

        public ASTNode GetCurrent(string instanceID)
        {
            return GetCurrentNode(instanceID);
        }

        public ASTNode GetCurrentPrevNode(string instanceID)
        {
            var current = GetCurrentNode(instanceID);
            return current.GetFromNode();
        }

        public WorkflowNode GetCurrentNode(string instanceID)
        {
            return WorkflowInstance.GetInstance(instanceID).Current;
        }

        public void UndoSubmit(string instanceID, long actorID, string actorName,string bussinessID)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            dynamic dynData = new ExpandoObject();
            dynData.bussinessID = bussinessID;
            dynData.Message = "撤销此节点";
            context.Cancel(new WorkflowContext()
            {
                Instance = instance,
                Data = dynData,
                ActorID = actorID,
                ActorName=actorName
            });
        }

        public void Rollback(string instanceID, long actorID, string actorName, dynamic data)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            context.Rollback(new WorkflowContext()
            {
                Instance = instance,
                Data = data,
                ActorID = actorID,
                ActorName = actorName
            });
        }

        public List<Group> GetCurrentActorGroup(string instanceID)
        {
            return WorkflowInstance.GetInstance(instanceID).Current.Groups;
        }

        public string Start(string identification)
        {
            return context.Start(identification);
        }

        public void Jump(string instanceID, string transitionID, long actorID, string actorName, dynamic data)
        {
            WorkflowInstance instance = WorkflowInstance.GetInstance(instanceID);
            context.Jump(new WorkflowContext()
            {
                Instance = instance,
                TransitionID = transitionID,
                Data = data,
                ActorID = actorID,
                ActorName = actorName
            });
        }
    }
}