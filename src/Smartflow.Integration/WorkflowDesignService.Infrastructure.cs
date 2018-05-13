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
        private  const string SQL_USER_PAGE = "SELECT TOP {0} ID,USERNAME,ORGCODE,ORGNAME,EMPLOYEENAME  FROM T_USER WHERE  ID NOT IN (SELECT TOP ({0}*({1}-1)) ID  FROM T_USER  WHERE 1=1 {2} ORDER BY ID) {2}";
        private  const string SQL_USER_PAGE_ROWCOUNT = "SELECT COUNT(*) FROM T_USER  WHERE 1=1";

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

        public IList<IEntry> GetUserList(int page, int rows, out int total, Dictionary<string, object> queryArg)
        {
            List<IEntry> entry = new List<IEntry>();
            string whereStr = SetQueryArg(queryArg);
            total = Connection.ExecuteScalar<int>(string.Format(" {0}{1} ", SQL_USER_PAGE_ROWCOUNT, whereStr));
            List<User> userList = Connection.Query<User>(string.Format(SQL_USER_PAGE, rows, page, whereStr)).ToList();
            entry.AddRange(userList);
            return entry;
        }

        private string SetQueryArg(Dictionary<string, object> queryArg)
        {
            StringBuilder whereStr = new StringBuilder();
            if (queryArg.ContainsKey("Code"))
            {
                whereStr.AppendFormat(" AND ORGCODE IN ({0}) ",GetOrganizationCodes(queryArg["Code"].ToString()));
            }
            if (queryArg.ContainsKey("SearchKey"))
            {
                whereStr.AppendFormat(" AND EMPLOYEENAME LIKE '{0}%'", queryArg["SearchKey"]);
            }

            if (queryArg.ContainsKey("UserIds"))
            {
                whereStr.AppendFormat(" AND ID NOT IN ({0}) ", queryArg["UserIds"]);
            }

            return whereStr.ToString();
        }

        private string GetOrganizationCodes(string code)
        {
            string[] codeArray = code.Split(',');
            List<string> codeList = new List<string>();
            foreach (string item in codeArray)
            {
                codeList.Add(string.Format("'{0}'", item));
            }

            return string.Join(",",codeList);
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
