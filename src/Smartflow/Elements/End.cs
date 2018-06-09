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
    public class End :Node
    {
        [XmlIgnore]
        public override string APPELLATION
        {
            get{ return "结束";}
        }

        [XmlIgnore]
        public override List<Transition> Transitions
        {
            get;
            set;
        }

        public override WorkflowNodeCategeory NodeType
        {
            get{return WorkflowNodeCategeory.End;}
        }
    }
}
