using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    public class Command : Element, IRelationShip
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        [XmlElement("text")]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 数据源ID
        /// </summary>
        [XmlElement("ds")]
        public string DSID
        {
            get;
            set;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlElement("connection")]
        public string CONNECTION
        {
            get;
            set;
        }

        [XmlElement("dbcategory")]
        public DatabaseCategory DBCATEGORY
        {
            get;
            set;
        }

        [XmlIgnore]
        public long RNID
        {
            get;
            set;
        }

        internal override void Persistent(string instanceID)
        {
            string sql = "INSERT INTO T_COMMAND(RNID,NAME,TEXT,CONNECTION,INSTANCEID,DSID,DBCATEGORY) VALUES(@RNID,@NAME,@TEXT,@CONNECTION,@INSTANCEID,@DSID,@DBCATEGORY)";
            Connection.Execute(sql, new
            {
                RNID = RNID,
                NAME = NAME,
                TEXT = Text,
                DSID = DSID,
                CONNECTION = CONNECTION,
                INSTANCEID = instanceID,
                DBCATEGORY = DBCATEGORY.ToString()
            });
        }
    }
}
