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
        public TreeNode GetOrganization()
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

        private string GetOrganizationCodes(string code)
        {
            string[] codeArray = code.Split(',');
            List<string> codeList = new List<string>();
            foreach (string item in codeArray)
            {
                codeList.Add(string.Format("'{0}'", item));
            }

            return string.Join(",", codeList);
        }

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
