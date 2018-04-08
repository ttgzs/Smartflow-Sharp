using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public  class DatabaseService
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
