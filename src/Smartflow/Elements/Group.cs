/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smartflow.Dapper;
using Smartflow.Enums;
using System.Xml.Serialization;

namespace Smartflow.Elements
{
    public class Group : Element, IRelationShip
    {
        public string RNID
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            string sql = "INSERT INTO T_GROUP(NID,IDENTIFICATION,RNID,APPELLATION,INSTANCEID) VALUES(@NID,@IDENTIFICATION,@RNID,@APPELLATION,@INSTANCEID)";
            Connection.Execute(sql, new
            {
                NID = Guid.NewGuid().ToString(),
                IDENTIFICATION = IDENTIFICATION,
                RNID = RNID,
                APPELLATION = APPELLATION,
                INSTANCEID = INSTANCEID
            });
        }
    }
}
