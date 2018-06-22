using System;
namespace Smartflow
{
    public interface ILogging
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);
    }
}
