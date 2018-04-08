using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dapper;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(List<Transition>))]
    public class Start : Node
    {
        [XmlIgnore]
        public override string NAME
        {
            get { return "开始"; }
        }

        public override WorkflowNodeCategeory NodeType
        {
            get
            {
                return WorkflowNodeCategeory.Start;
            }
        }
    }
}
