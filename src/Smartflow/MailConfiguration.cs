using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class MailConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get { return this["account"].ToString(); }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"].ToString(); }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"].ToString(); }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return int.Parse(this["port"].ToString()); }
        }

        [ConfigurationProperty("enableSsl", IsRequired = true)]
        public bool EnableSsl
        {
            get { return bool.Parse(this["enableSsl"].ToString()); }
        }
    }
}
