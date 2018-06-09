/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(List<Transition>))]
    public class Start : Node
    {
        [XmlIgnore]
        public override string APPELLATION
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
