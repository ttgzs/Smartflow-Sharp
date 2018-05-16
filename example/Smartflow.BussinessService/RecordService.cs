using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Smartflow.BussinessService.Models;

namespace Smartflow.BussinessService
{
    public class RecordService
    {
        protected IDbConnection Connection
        {
            get { return SqlHelper.CreateConnection(); }
        }

        public void Persistent(Record model)
        {
            string sql = "INSERT INTO T_RECORD(NODENAME,MESSAGE,INSTANCEID) VALUES (@NODENAME,@MESSAGE,@INSTANCEID)";
            Connection.Execute(sql, model);
        }

        public List<Record> Query(string WFID)
        {
            string sql = " SELECT * FROM T_RECORD WHERE INSTANCEID=@INSTANCEID ORDER BY INSERTDATE ASC ";
            return Connection.Query<Record>(sql, new
            {
                INSTANCEID = WFID

            }).ToList();
        }
    }
}