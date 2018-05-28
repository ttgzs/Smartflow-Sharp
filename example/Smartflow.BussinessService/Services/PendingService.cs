using Smartflow.BussinessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.BussinessService.Services
{
    public class PendingService:BaseService
    {
        public void Persistent(Pending model)
        {
            string sql = "INSERT INTO T_PENDING(ACTORID,NODEID,INSTANCEID,NAME,ACTION) VALUES (@ACTORID,@NODEID,@INSTANCEID,@NAME,@ACTION)";
            Connection.Execute(sql, model);
        }
    }
}
