using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Dapper;
using Smartflow.Integration.Models;

namespace Smartflow.Integration
{
    public partial class WorkflowDesignService
    {
        public TreeNode GetOrganizationTree()
        {
            string query = "SELECT * FROM T_ORG WHERE PARENTCODE=@PARENTCODE";

            TreeNode root = Connection.Query<TreeNode>(query, new
            {
                PARENTCODE = "0"

            }).FirstOrDefault();

            EachNode(root);

            return root;
        }

        private void EachNode(TreeNode node)
        {
            string query = "SELECT * FROM T_ORG WHERE PARENTCODE=@PARENTCODE";

            List<TreeNode> childNode = Connection.Query<TreeNode>(query, new
            {
                PARENTCODE = node.Code

            }).ToList();

            if (childNode.Count > 0)
            {
                node.Children = new List<TreeNode>();
            }

            foreach (TreeNode n in childNode)
            {
                EachNode(n);
                node.Children.Add(n);
            }
        }

        public IList<IEntry> GetUserByOrganizationId(string organizationCode)
        {
            return null;
        }

        public IList<IEntry> GetUserList(int page, int rows, out int total, Dictionary<string, object> query)
        {
            List<IEntry> entry = new List<IEntry>();
            string pageQuery = "SELECT Top {0} ID,UserName,OrgCode,OrgName,EmployeeName  FROM T_USER Where  ID Not in (Select Top ({0}*({1}-1)) ID  From T_USER  Where 1=1 {2} Order by ID) {2}";
            string totalSql = " SELECT * FROM T_USER  WHERE 1=1 ";
            StringBuilder whereStr = new StringBuilder();
            if (query.ContainsKey("code"))
            {
                whereStr.AppendFormat(" AND OrgCode Like '{0}%'", query["code"]);
            }

            if (query.ContainsKey("key"))
            {
                whereStr.AppendFormat(" AND EmployeeName Like '{0}%'", query["key"]);
            }

            if (query.ContainsKey("userIdStr"))
            {
                whereStr.AppendFormat(" AND ID NOT IN ({0}) ", query["userIdStr"]);
            }

            total = Connection.ExecuteScalar<int>(string.Format(" {0}{1} ", totalSql, whereStr));
            List<User> userList =
                Connection.Query<User>(string.Format(pageQuery, rows, page, whereStr)).ToList();

            entry.AddRange(userList);
            return entry;
        }

        public IList<IEntry> GetUserByRoleId(string roleId)
        {
            return null;
        }

        public IList<IEntry> GetRole(string roleIds)
        {
            List<IEntry> entry = new List<IEntry>();
            string query = " SELECT * FROM T_ROLE WHERE 1=1 ";

            if (!String.IsNullOrEmpty(roleIds))
            {
                query = query + " AND ID NOT IN (" + roleIds + ")";
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
