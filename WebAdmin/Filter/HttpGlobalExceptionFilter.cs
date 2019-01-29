using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using NLog.Fluent;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private static Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        /// <summary>
        /// 当系统发生未捕捉的异常时就会触发这个方法
        /// </summary>
        /// <param name="context">context是上下文，包含了错误异常信息</param>
        public void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception, () => { return "在全局捕获异常:" + context.Exception.Message; });
        }
    }
}
