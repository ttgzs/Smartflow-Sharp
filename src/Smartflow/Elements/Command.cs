/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Smartflow.Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    public class Command : Element, IRelationShip
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        [XmlElement("script")]
        public string SCRIPT
        {
            get;
            set;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlElement("connecte")]
        public string CONNECTE
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


        [XmlElement("commandtype")]
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
            string sql = "INSERT INTO T_COMMAND(NID,RNID,APPELLATION,SCRIPT,CONNECTE,INSTANCEID,DBCATEGORY,COMMANDTYPE) VALUES(@NID,@RNID,@APPELLATION,@SCRIPT,@CONNECTE,@INSTANCEID,@DBCATEGORY,@COMMANDTYPE)";
            Connection.Execute(sql, new
            {
                NID=Guid.NewGuid().ToString(),
                RNID = RNID,
                APPELLATION = APPELLATION,
                SCRIPT = SCRIPT,
                CONNECTE = CONNECTE,
                INSTANCEID = INSTANCEID,
                DBCATEGORY = DBCATEGORY.ToString(),
                COMMANDTYPE = COMMANDTYPE
            });
        }
    }
}
