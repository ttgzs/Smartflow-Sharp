using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Infrastructure;
using Smartflow.DesignService.Models;

namespace Smartflow.DesignService
{
    public partial class WorkflowDesignService
    {
        public IList<IEntry> GetRole(string roleIds)
        {
            List<IEntry> entry = new List<IEntry>();
            string query = " SELECT * FROM T_ROLE WHERE 1=1 ";

            if (!String.IsNullOrEmpty(roleIds))
            {
                query = string.Format("{0} AND ID NOT IN ({1})", query, roleIds);
            }
            entry.AddRange(Connection.Query<Role>(query).ToList());
            return entry;
        }

        public IList<IEntry> GetConfigs()
        {
            List<IEntry> entry = new List<IEntry>();
            string query = " SELECT * FROM T_CONFIG ";

            entry.AddRange(Connection.Query<Config>(query).ToList());
            return entry;
        }
    }
}
