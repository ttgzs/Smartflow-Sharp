using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;

namespace Smartflow.Web.Code
{
    public class ApplyService
    {
        public IDbConnection Connection
        {
            get { return DapperHelper.CreateWorkflowConnection(); }
        }

        public void Persistent(Apply model)
        {
            string sql = "INSERT INTO T_APPLY(STATE,NAME,DESCRIPTION,WFID,INSTANCEID) VALUES (@STATE,@NAME,@DESCRIPTION,@WFID,@INSTANCEID)";
            Connection.Execute(sql, model);
        }

        public void Update(Apply model)
        {
            string sql = " UPDATE T_APPLY SET NAME=@NAME,STATE=@STATE,DESCRIPTION=@DESCRIPTION,WFID=@WFID,INSTANCEID=@INSTANCEID WHERE AID=@AID";
            Connection.Execute(sql, model);
        }

        public void Delete(long autoID)
        {
            string sql = " DELETE FROM T_APPLY WHERE AID=@ID ";
            Connection.Execute(sql, new
            {
                ID = autoID
            });
        }

        public List<Apply> Query()
        {
            string sql = " SELECT * FROM T_APPLY ";
            return Connection.Query<Apply>(sql).ToList();
        }

        public Apply GetInstance(long autoID)
        {
            string sql = "SELECT * FROM T_APPLY WHERE AID=@ID";

            return Connection.Query<Apply>(sql, new { ID = autoID })
                .FirstOrDefault<Apply>();
        }
    }
}