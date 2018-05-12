/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Note: to build on C# 3.0 + .NET 4.0
 Author:chengderen-237552006@qq.com
 */
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


        [XmlElement("COMMANDTYPE")]
        public string COMMANDTYPE
        {
            get;
            set;
        }


        [XmlIgnore]
        public string RNID
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_COMMAND(NID,RNID,NAME,TEXT,CONNECTION,INSTANCEID,DBCATEGORY,COMMANDTYPE) VALUES(@NID,@RNID,@NAME,@TEXT,@CONNECTION,@INSTANCEID,@DBCATEGORY,@COMMANDTYPE)";
            Connection.Execute(sql, new
            {
                NID=Guid.NewGuid().ToString(),
                RNID = RNID,
                NAME = NAME,
                TEXT = Text,
                CONNECTION = CONNECTION,
                INSTANCEID = INSTANCEID,
                DBCATEGORY = DBCATEGORY.ToString(),
                COMANDTYPE = COMMANDTYPE
            });
        }
    }
}
