using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class SmartflowConfigHandle : ConfigurationSection
    {
        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return this["connectionString"].ToString(); }
        }

        [ConfigurationProperty("category", IsRequired = true, DefaultValue = "SQLServer")]
        public string DatabaseCategory
        {
            get { return this["category"].ToString(); }
        }
    }
}
