using System;
namespace Smartflow
{
    public interface ILogging
    {
        void WriteLog(Exception ex);
        void WriteLog(string message);
    }
}
