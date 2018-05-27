using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService.Services
{
    public class BaseService
    {
        public IDbConnection Connection
        {
            get { return SqlHelper.CreateConnection(); }
        }
    }
}
