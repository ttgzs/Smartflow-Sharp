/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Smartflow.Elements;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(Start))]
    [XmlInclude(typeof(End))]
    [XmlInclude(typeof(List<Node>))]
    [XmlInclude(typeof(List<Decision>))]
    [XmlRoot("workflow")]
    [XmlType("Workflow")]
    public class Workflow
    {
        /// <summary>
        /// 开始节点
        /// </summary>
        [XmlElement(ElementName = "start")]
        public Start StartNode
        {
            get;
            set;
        }

        /// <summary>
        /// 结束节点
        /// </summary>
        [XmlElement(ElementName = "end")]
        public End EndNode
        {
            get;
            set;
        }

        /// <summary>
        /// 决策节点
        /// </summary>
        [XmlElement(ElementName = "decision")]
        public List<Decision> ChildDecisionNode
        {
            get;
            set;
        }

        /// <summary>
        /// 流程节点
        /// </summary>
        [XmlElement(ElementName = "node")]
        public List<Node> ChildNode
        {
            get;
            set;
        }
    }
}
