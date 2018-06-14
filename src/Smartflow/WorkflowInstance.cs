/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smartflow.Elements;
using Smartflow.Dapper;
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

        public string JSSTRUCTURE
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

            string sql = " SELECT X.INSTANCEID,X.RNID,X.JSSTRUCTURE,Y.APPELLATION,Y.NID,Y.IDENTIFICATION,Y.NODETYPE,Y.INSTANCEID FROM T_INSTANCE X INNER JOIN  T_NODE Y  ON (X.INSTANCEID=Y.INSTANCEID AND X.RNID=Y.IDENTIFICATION) WHERE X.INSTANCEID=@INSTANCEID";

            try
            {
                workflowInstance = workflowInstance.Connection.Query<WorkflowInstance, ASTNode, WorkflowInstance>(sql, (instance, node) =>
                {
                    instance.Current = WorkflowNode.ConvertToReallyType(node);
                    return instance;

                }, param: new { INSTANCEID = instanceID }, splitOn: "APPELLATION").FirstOrDefault<WorkflowInstance>();

                return workflowInstance;
            }
            catch (Exception ex)
            {
                throw new WorkflowException(ex, instanceID);
            }
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

        internal static string CreateWorkflowInstance(long nodeID, string flowID,string flowImage)
        {
            string instanceID = Guid.NewGuid().ToString();
            string sql = "INSERT INTO T_INSTANCE(INSTANCEID,RNID,FLOWID,STATE,JSSTRUCTURE) VALUES(@INSTANCEID,@RNID,@FLOWID,@STATE,@JSSTRUCTURE)";

            DapperFactory.CreateWorkflowConnection().Execute(sql, new
            {
                INSTANCEID = instanceID,
                RNID = nodeID,
                FLOWID = flowID,
                STATE = WorkflowInstanceState.Running.ToString(),
                JSSTRUCTURE = flowImage
            });

            return instanceID;
        }
    }
}
