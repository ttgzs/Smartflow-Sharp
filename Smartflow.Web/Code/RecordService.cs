using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;

namespace Smartflow.Web.Code
{
    public class RecordService
    {
        public IDbConnection Connection
        {
            get { return DapperHelper.CreateWorkflowConnection(); }
        }

        public void Persistent(Record model)
        {
            string sql = "INSERT INTO T_RECORD(NODENAME,MESSAGE,INSTANCEID) VALUES (@NODENAME,@MESSAGE,@INSTANCEID)";
            Connection.Execute(sql, model);
        }

        public List<Record> Query(string WFID)
        {
            string sql = " SELECT * FROM T_RECORD WHERE INSTANCEID=@INSTANCEID ";
            return Connection.Query<Record>(sql, new
            {
                INSTANCEID = WFID

            }).ToList();
        }
    }
}