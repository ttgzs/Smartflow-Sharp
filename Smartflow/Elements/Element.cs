using Smartflow.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Smartflow.Elements
{
    public abstract class Element 
    {
        [XmlAttribute("id")]
        public virtual string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 节点唯一标识
        /// </summary>
        [XmlIgnore]
        public long NID
        {
            get;
            set;
        }

        [XmlAttribute("name")]
        public virtual string NAME
        {
            get;
            set;
        }


        [XmlIgnore]
        public virtual string INSTANCEID
        {
            get;
            set;
        }

        internal abstract void Persistent(string instanceID);

        protected IDbConnection Connection
        {
            get { return DapperFactory.CreateWorkflowConnection(); }
        }

    }
}
