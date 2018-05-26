/**
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Smartflow
{
    internal  class DatabaseService
    {
        public IDbConnection connection
        {
            get;
            set;
        }

        protected DatabaseService(IDbConnection connection)
        {
            this.connection = connection;
        }

        public static IDbConnection CreateInstance(IDbConnection connection)
        {
            return new DatabaseService(connection).connection;
        }
    }
}
