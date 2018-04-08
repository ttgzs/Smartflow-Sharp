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
        public virtual List<Actor> Actors
        {
            get;
            set;
        }

        [XmlElement(ElementName = "role")]
        public virtual List<Role> Roles
        {
            get;
            set;
        }

        internal override void Persistent(string instanceID)
        {
            base.Persistent(instanceID);

            if (Transitions != null)
            {
                foreach (Transition transition in Transitions)
                {
                    transition.RNID = this.NID;
                    transition.Persistent(instanceID);
                }
            }

            if (Actors != null)
            {
                foreach (Actor actor in Actors)
                {
                    actor.RNID = this.NID;
                    actor.Persistent(instanceID);
                }
            }

            if (Roles != null)
            {
                foreach (Role r in Roles)
                {
                    r.RNID = this.NID;
                    r.Persistent(instanceID);
                }
            }
        }
    }
}
