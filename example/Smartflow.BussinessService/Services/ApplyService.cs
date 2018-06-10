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
            if (model.IDENTIFICATION == 0)
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
            string sql = "INSERT INTO T_APPLY(STATUS,FNAME,DESCRIPTION,STRUCTUREID,INSTANCEID,SECRETGRADE) VALUES (@STATUS,@FNAME,@DESCRIPTION,@STRUCTUREID,@INSTANCEID,@SECRETGRADE)";
            Connection.Execute(sql, model);
        }

        protected void Update(Apply model)
        {
            string sql = "UPDATE T_APPLY SET FNAME=@FNAME,STATUS=@STATUS,DESCRIPTION=@DESCRIPTION,STRUCTUREID=@STRUCTUREID,INSTANCEID=@INSTANCEID,SECRETGRADE=@SECRETGRADE WHERE IDENTIFICATION=@IDENTIFICATION";
            Connection.Execute(sql, model);
        }

        public void Delete(long IDENTIFICATION)
        {
            string sql = " DELETE FROM T_APPLY WHERE IDENTIFICATION=@IDENTIFICATION ";
            Connection.Execute(sql, new
            {
                IDENTIFICATION = IDENTIFICATION
            });
        }

        public List<Apply> Query()
        {
            string sql = " SELECT * FROM T_APPLY ";
            return Connection.Query<Apply>(sql).ToList();
        }

        public Apply GetInstance(long IDENTIFICATION)
        {
            string sql = "SELECT * FROM T_APPLY WHERE IDENTIFICATION=@IDENTIFICATION";

            return Connection.Query<Apply>(sql, new { IDENTIFICATION = IDENTIFICATION })
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