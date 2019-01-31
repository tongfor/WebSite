using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// 递归记录内层Exception日志
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogInnerError(this ILogger logger, Exception exception, string message)
        {
            if (exception.InnerException != null)
            {
                LogInnerError(logger, exception.InnerException, exception.InnerException.Message);
            }
            else
            {
                logger.LogError(exception, message);
            }
        }
    }
}
