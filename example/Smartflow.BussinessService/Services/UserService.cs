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
    }
}
