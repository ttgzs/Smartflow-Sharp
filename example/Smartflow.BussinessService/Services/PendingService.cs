using Smartflow.BussinessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.BussinessService.Services
{
    public class PendingService : BaseService
    {
        public void Persistent(Pending model)
        {
            string sql = "INSERT INTO T_PENDING(ACTORID,NODEID,INSTANCEID,APPELLATION,ACTION) VALUES (@ACTORID,@NODEID,@INSTANCEID,@APPELLATION,@ACTION)";
            Connection.Execute(sql, model);
        }
        public List<Pending> Query(long actorID)
        {
            string sql = " SELECT * FROM T_PENDING WHERE ACTORID=@ACTORID ";
            return Connection.Query<Pending>(sql, new { ACTORID = actorID }).ToList();
        }

        public void Delete(string NODEID, string INSTANCEID)
        {
            string sql = " DELETE FROM T_PENDING WHERE NODEID=@NODEID AND INSTANCEID=@INSTANCEID ";
            Connection.Execute(sql, new { NODEID = NODEID, INSTANCEID = INSTANCEID });
        }

        public void Delete(string INSTANCEID)
        {
            string sql = " DELETE FROM T_PENDING WHERE INSTANCEID=@INSTANCEID ";
            Connection.Execute(sql, new { INSTANCEID = INSTANCEID });
        }

        public bool Check(string actorID, string NODEID, string INSTANCEID)
        {
            string sql = " SELECT * FROM T_PENDING WHERE ACTORID=@ACTORID AND NODEID=@NODEID AND INSTANCEID=@INSTANCEID ";

            return (Connection.Query<Pending>(sql, 
                new { ACTORID = actorID, 
                      NODEID = NODEID, 
                      INSTANCEID = INSTANCEID }).FirstOrDefault() != null);
        }
    }
}
