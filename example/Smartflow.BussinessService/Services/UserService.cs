using Smartflow.BussinessService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;

namespace Smartflow.BussinessService.Services
{
    public class UserService : BaseService
    {
        public List<User> GetUserList(string roleIDs)
        {
            string executeSql = @"SELECT * FROM T_USER WHERE ID IN (SELECT UUID FROM T_UMR  WHERE RID IN (" + roleIDs + "))";
            return Connection.Query<User>(executeSql).ToList();
        }

        public User GetUser(string userName)
        {
            string executeSql = @"SELECT * FROM T_USER WHERE USERNAME=@USERNAME";
            return Connection.Query<User>(executeSql, new
            {
                USERNAME = userName
            }).FirstOrDefault();
        }

        public List<User> GetUserList()
        {
            string executeSql = @" SELECT * FROM T_USER WHERE 1=1 ";
            return Connection.Query<User>(executeSql).ToList();
        }

        public DataTable GetStatisticsDataTable()
        {
            string executeSql = @" SELECT USERNAME,EMPLOYEENAME,Z.APPELLATION FROM T_UMR X,T_USER Y,T_ROLE  Z WHERE X.RID=Z.IDENTIFICATION AND Y.ID=X.UUID  ORDER BY Y.ID ";
            DataTable dt = new DataTable();
            using (IDataReader dr = Connection.ExecuteReader(executeSql))
            {
                dt.Load(dr);
            }
            return dt;
        }


        public List<User> GetPendingUserList(string nodeID, string instanceID)
        {
            string executeSql = @" SELECT * FROM T_USER WHERE ID IN (SELECT ACTORID FROM DBO.T_PENDING WHERE NODEID=@NODEID AND INSTANCEID=@INSTANCEID) ";
            return Connection.Query<User>(executeSql, new
            {
                NODEID = nodeID,
                INSTANCEID = instanceID

            }).ToList();
        }
    }
}
