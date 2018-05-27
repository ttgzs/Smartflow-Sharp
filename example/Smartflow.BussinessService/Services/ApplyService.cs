using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Smartflow.BussinessService.Models;

namespace Smartflow.BussinessService.Services
{
    public class ApplyService : BaseService
    {
        public void Persistent(Apply model)
        {
            if (model.AUTOID == 0)
            {
                Insert(model);
            }
            else
            {
                Update(model);
            }
        }

        protected void Insert(Apply model)
        {
            string sql = "INSERT INTO T_APPLY(STATUS,NAME,DESCRIPTION,WFID,INSTANCEID,SECRETGRADE) VALUES (@STATUS,@NAME,@DESCRIPTION,@WFID,@INSTANCEID,@SECRETGRADE)";
            Connection.Execute(sql, model);
        }

        protected void Update(Apply model)
        {
            string sql = "UPDATE T_APPLY SET NAME=@NAME,STATUS=@STATUS,DESCRIPTION=@DESCRIPTION,WFID=@WFID,INSTANCEID=@INSTANCEID,SECRETGRADE=@SECRETGRADE WHERE ID=@ID";
            Connection.Execute(sql, model);
        }

        public void Delete(long autoID)
        {
            string sql = " DELETE FROM T_APPLY WHERE AUTOID=@AUTOID ";
            Connection.Execute(sql, new
            {
                AUTOID = autoID
            });
        }

        public List<Apply> Query()
        {
            string sql = " SELECT * FROM T_APPLY ";
            return Connection.Query<Apply>(sql).ToList();
        }

        public Apply GetInstance(long autoID)
        {
            string sql = "SELECT * FROM T_APPLY WHERE AUTOID=@AUTOID";

            return Connection.Query<Apply>(sql, new { AUTOID = autoID })
                .FirstOrDefault<Apply>();
        }

        public Apply GetInstanceByInstanceID(string INSTANCEID)
        {
            string sql = "SELECT * FROM T_APPLY WHERE INSTANCEID=@INSTANCEID";

            return Connection.Query<Apply>(sql, new { INSTANCEID = INSTANCEID })
                .FirstOrDefault<Apply>();
        }
    }
}