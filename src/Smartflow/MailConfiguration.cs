using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class MailConfiguration: ConfigurationSection
    {
        public string Account
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool EnableSsl
        {
            get;
            set;
        }
    }
}
