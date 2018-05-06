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


        public IList<IEntry> GetUserList(string searchKey, string organizationCode)
        {
            List<IEntry> entry = new List<IEntry>();
            string query = " SELECT * FROM T_USER ";

            List<User> userList =
                Connection.Query<User>(query).ToList();

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
    }
}
