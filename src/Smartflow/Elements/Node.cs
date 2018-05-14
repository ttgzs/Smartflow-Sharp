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
    public class Node : ASTNode
    {
        private WorkflowNodeCategeory _nodeType = WorkflowNodeCategeory.Normal;

        public override WorkflowNodeCategeory NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        [XmlElement(ElementName = "actor")]
        internal virtual List<Actor> Actors
        {
            get;
            set;
        }

        [XmlElement(ElementName = "group")]
        internal virtual List<Group> MultiGroup
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

            if (Actors != null)
            {
                foreach (Actor actor in Actors)
                {
                    actor.RNID = this.NID;
                    actor.INSTANCEID = INSTANCEID;
                    actor.Persistent();
                }
            }

            if (MultiGroup != null)
            {
                foreach (Group r in MultiGroup)
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
