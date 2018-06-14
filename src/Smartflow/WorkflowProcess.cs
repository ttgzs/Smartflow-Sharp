/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smartflow.Dapper;
using Smartflow.Enums;
using System.Data;

namespace Smartflow
{
    public class WorkflowProcess : IPersistent, IRelationShip
    {
        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
        }

        /// <summary>
        /// 外键
        /// </summary>
        public string RNID
        {
            get;
            set;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string NID
        {
            get;
            set;
        }

        /// <summary>
        /// 当前节点
        /// </summary>
        public long ORIGIN
        {
            get;
            set;
        }

        /// <summary>
        /// 跳转到的节点
        /// </summary>
        public long DESTINATION
        {
            get;
            set;
        }

        /// <summary>
        /// 路线ID
        /// </summary>
        public string TRANSITIONID
        {
            get;
            set;
        }

        /// <summary>
        /// 实例ID
        /// </summary>
        public string INSTANCEID
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public WorkflowNodeCategeory NODETYPE
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CREATEDATETIME
        {
            get;
            set;
        }

        /// <summary>
        /// 退回、撤销、跳转
        /// </summary>
        public WorkflowAction OPERATION
        {
            get;
            set;
        }

        /// <summary>
        /// 将数据持久到数据库
        /// </summary>
        public void Persistent()
        {
            string sql = "INSERT INTO T_PROCESS(NID,ORIGIN,DESTINATION,TRANSITIONID,INSTANCEID,NODETYPE,RNID,OPERATION) VALUES(@NID,@ORIGIN,@DESTINATION,@TRANSITIONID,@INSTANCEID,@NODETYPE,@RNID,@OPERATION)";
            Connection.Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                ORIGIN = ORIGIN,
                DESTINATION = DESTINATION,
                TRANSITIONID = TRANSITIONID,
                INSTANCEID = INSTANCEID,
                NODETYPE = NODETYPE.ToString(),
                RNID = RNID,
                OPERATION = OPERATION
            });
        }

        public static WorkflowProcess GetWorkflowProcessInstance(string instanceID, string NID)
        {
            WorkflowProcess instance = new WorkflowProcess();
            string query = " SELECT * FROM T_PROCESS WHERE INSTANCEID=@INSTANCEID AND RNID=@NID  AND OPERATION=@OPERATION ";
            instance = instance.Connection.Query<WorkflowProcess>(query, new
            {
                INSTANCEID = instanceID,
                NID = NID,
                OPERATION = WorkflowAction.Jump

            }).OrderByDescending(order => order.CREATEDATETIME).FirstOrDefault();

            return instance;
        }
    }
}
