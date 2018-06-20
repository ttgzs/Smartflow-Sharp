using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Smartflow
{
    public class MailService : IMailService
    {
        private const string CONST_MAIL_URL_EXPRESSION = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
        private SmtpClient _smtp = new SmtpClient();

        private MailConfiguration mailConfiguration =
                ConfigurationManager.GetSection("mailConfiguration") as MailConfiguration;

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

        public MailService()
        {
            if (mailConfiguration == null) return;

            this._smtp.Host = mailConfiguration.Host;
            this._smtp.Port = mailConfiguration.Port;
            this._smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            this._smtp.EnableSsl = mailConfiguration.EnableSsl;
            this.Account = mailConfiguration.Account;
            this.Password = mailConfiguration.Password;
            //ren421
            this._smtp.Credentials = new NetworkCredential(mailConfiguration.Account, this.Password);

        }

        public void Notification(string[] to,string body)
        {
            this.Send(this.Account, this.Account, to, "待办通知", body);
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人邮件地址</param>
        /// <param name="sender">发件人显示名称</param>
        /// <param name="to">收件人地址</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件正文</param>
        protected void Send(string from, string sender, string[] recvierArray, string subject, string body)
        {
            if (recvierArray.Any(MAddress => !Regex.IsMatch(MAddress, MailService.CONST_MAIL_URL_EXPRESSION)))
                return;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(from, sender);
            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.Normal;
            foreach (string recvier in recvierArray)
            {
                message.To.Add(recvier);
            }
            try
            {
                this._smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(ex);
            }
        }
    }
}
