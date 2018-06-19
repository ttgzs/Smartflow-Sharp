using System;
namespace Smartflow
{
    public interface IMailService
    {
        void Notification(string[] to, string body);
    }
}
