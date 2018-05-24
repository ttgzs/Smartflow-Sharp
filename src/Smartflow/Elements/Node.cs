/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(List<Transition>))]
    [XmlInclude(typeof(List<Group>))]
    public class Node : ASTNode
    {
        private WorkflowNodeCategeory _nodeType = WorkflowNodeCategeory.Normal;

        public override WorkflowNodeCategeory NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        [XmlElement(ElementName = "group")]
        public virtual List<Group> Groups
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            base.Persistent();

            if (Transitions != null)
            {
                foreach (Transition transition in Transitions)
                {
                    transition.RNID = this.NID;
                    transition.SOURCE = this.ID;
                    transition.INSTANCEID = INSTANCEID;
                    transition.Persistent();
                }
            }

            if (Groups!= null)
            {
                foreach (Group r in Groups)
                {
                    r.RNID = this.NID;
                    r.INSTANCEID = INSTANCEID;
                    r.Persistent();
                }
            }
        }

        internal ASTNode GetNode(long ID)
        {
            string query = "SELECT * FROM T_NODE WHERE ID=@ID AND INSTANCEID=@INSTANCEID";

            ASTNode node = DapperFactory.CreateWorkflowConnection().Query<ASTNode>(query, new
            {
                ID = ID,
                INSTANCEID = INSTANCEID

            }).FirstOrDefault();

            return node;
        }
    }
}
