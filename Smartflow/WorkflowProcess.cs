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
using Dapper;
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
        public long FROM
        {
            get;
            set;
        }

        /// <summary>
        /// 跳转到的节点
        /// </summary>
        public long TO
        {
            get;
            set;
        }

        /// <summary>
        /// 路线ID
        /// </summary>
        public string TID
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
        /// 将数据持久到数据库
        /// </summary>
        public void Persistent()
        {
            string sql = "INSERT INTO T_PROCESS(NID,[FROM],[TO],TID,INSTANCEID,NODETYPE,RNID) VALUES(@NID,@FROM,@TO,@TID,@INSTANCEID,@NODETYPE,@RNID)";
            Connection.Execute(sql, new
            {
                NID=Guid.NewGuid().ToString(),
                FROM = FROM,
                TO = TO,
                TID = TID,
                INSTANCEID = INSTANCEID,
                NODETYPE = NODETYPE.ToString(),
                RNID = RNID
            });
        }

        public static WorkflowProcess GetWorkflowProcessInstance(string instanceID, string NID)
        {
            WorkflowProcess instance = new WorkflowProcess();
            //兼容其它数据库
            string query = " SELECT * FROM T_PROCESS WHERE INSTANCEID=@INSTANCEID AND RNID=@NID  ";
            instance = instance.Connection.Query<WorkflowProcess>(query, new
            {
                INSTANCEID = instanceID,
                NID = NID

            }).OrderBy(order => order.CREATEDATETIME).FirstOrDefault();

            return instance;
        }
    }
}
