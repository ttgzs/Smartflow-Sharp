/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Infrastructure;

namespace Smartflow.DesignService
{
    public partial class WorkflowDesignService
    {
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

        public DataTable GetConfigs()
        {
            string query = " SELECT * FROM T_CONFIG ";
            DataTable configData = new DataTable(Guid.NewGuid().ToString());
            using (IDataReader dr = Connection.ExecuteReader(query))
            {
                configData.Load(dr);
            }
            return configData;
        }
    }
}
