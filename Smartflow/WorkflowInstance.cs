/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smartflow.Elements;
using Dapper;
using System.Data;
using Smartflow.Enums;

namespace Smartflow
{
    public class WorkflowInstance
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
        }

        protected WorkflowInstance()
        {
        }

        public WorkflowInstanceState State
        {
            get;
            set;
        }

        public WorkflowNode Current
        {
            get;
            set;
        }

        public string InstanceID
        {
            get;
            set;
        }

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="instanceID">实例ID</param>
        /// <returns>流程实例</returns>
        public static WorkflowInstance GetInstance(string instanceID)
        {
            WorkflowInstance workflowInstance = new WorkflowInstance();
            workflowInstance.InstanceID = instanceID;
            
            string sql = " SELECT X.INSTANCEID,X.RNID,Y.NAME,Y.NID,Y.ID,Y.NODETYPE,Y.INSTANCEID FROM T_INSTANCE X INNER JOIN  T_NODE Y  ON (X.INSTANCEID=Y.INSTANCEID AND X.RNID=Y.ID ) WHERE X.INSTANCEID=@INSTANCEID";

            workflowInstance = workflowInstance.Connection.Query<WorkflowInstance, ASTNode, WorkflowInstance>(sql, (instance, node) =>
            {
                instance.Current= WorkflowNode.GetWorkflowNodeInstance(node);
                return instance;

            }, param: new { INSTANCEID = instanceID }, splitOn: "NAME").FirstOrDefault<WorkflowInstance>();

            return workflowInstance;
        }

        /// <summary>
        /// 进行流程跳转
        /// </summary>
        /// <param name="transitionTo">选择跳转路线</param>
        internal void Jump(long transitionTo)
        {
       
            string update = " UPDATE T_INSTANCE SET RNID=@RNID WHERE INSTANCEID=@INSTANCEID ";

            Connection.Execute(update, new
            {
                RNID = transitionTo,
                INSTANCEID = InstanceID
            });
        }

        /// <summary>
        /// 状态转换
        /// </summary>
        internal void Transfer()
        {
           
            string update = " UPDATE T_INSTANCE SET STATE=@STATE WHERE INSTANCEID=@INSTANCEID ";

            Connection.Execute(update, new
            {
                STATE = State.ToString(),
                INSTANCEID = InstanceID
            });
        }

        internal static string CreateWorkflowInstance(long nodeID, string flowID)
        {
            string instanceID = Guid.NewGuid().ToString();
            string sql = "INSERT INTO T_INSTANCE(INSTANCEID,RNID,FLOWID,STATE) VALUES(@INSTANCEID,@RNID,@FLOWID,@STATE)";

            DapperFactory.CreateWorkflowConnection().Execute(sql, new
            {
                INSTANCEID = instanceID,
                RNID = nodeID,
                FLOWID = flowID,
                STATE = WorkflowInstanceState.Running.ToString()
            });

            return instanceID;
        }
    }
}
