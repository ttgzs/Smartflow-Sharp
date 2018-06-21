using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Smartflow
{
    public class MailConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 账户名
        /// </summary>
        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get { return this["account"].ToString(); }
        }

        /// <summary>
        /// 密码（授权码）
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"].ToString(); }
        }

        /// <summary>
        /// 发送邮件显示的名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
        }


        /// <summary>
        /// 服务器smtp.163.com
        /// </summary>
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"].ToString(); }
        }

        /// <summary>
        /// 端口(25)
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return int.Parse(this["port"].ToString()); }
        }

        /// <summary>
        /// 启用HTTPS
        /// </summary>
        [ConfigurationProperty("enableSsl", IsRequired = true)]
        public bool EnableSsl
        {
            get { return bool.Parse(this["enableSsl"].ToString()); }
        }
    }
}
