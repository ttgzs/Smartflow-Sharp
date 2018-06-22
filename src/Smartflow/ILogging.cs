using System;
namespace Smartflow
{
    public interface ILogging
    {
        void Write(Exception ex);
        void Write(string message);
    }
}
