/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Smartflow.Dapper;
using Smartflow.Enums;
using System.Configuration;

namespace Smartflow.Web.Design
{
    public class ActorService
    {
        private IDbConnection Connection = DapperFactory.CreateConnection(DatabaseCategory.SQLServer,
            ConfigurationManager.AppSettings["busConnection"]);

        public DataTable GetRole(string roleIds, string searchKey)
        {
            string query = " SELECT * FROM T_ROLE WHERE 1=1 ";
            if (!String.IsNullOrEmpty(roleIds))
            {
                query = string.Format("{0} AND IDENTIFICATION NOT IN ({1})", query, roleIds);
            }

            if (!String.IsNullOrEmpty(searchKey))
            {
                query = string.Format("{0} AND APPELLATION LIKE '%{1}%'", query, searchKey);
            }
            
            DataTable roleData = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader dr = Connection.ExecuteReader(query))
            {
                roleData.Load(dr);
            }
            return roleData;
        }
    }
}
