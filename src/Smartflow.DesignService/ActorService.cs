/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Dapper;

namespace Smartflow.DesignService
{
    public class ActorService
    {
        private IDbConnection Connection = DbHelper.CreateConnection();

        public DataTable GetRole(string roleIds)
        {
            string query = " SELECT * FROM T_ROLE WHERE 1=1 ";
            if (!String.IsNullOrEmpty(roleIds))
            {
                query = string.Format("{0} AND IDENTIFICATION NOT IN ({1})", query, roleIds);
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
