/** 
* TimedExecutService.cs
*
* 功 能： 定时执行服务
* 类 名： TimedExecutService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/2/18 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Common.Config;
using IBLL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BLL
{
    public class TimedExecutService// : BackgroundService
    {
        /// <summary>
        /// 本站设置
        /// </summary>
        private readonly SiteConfig _siteConfig;

        private readonly GatherConfig _gatherConfig;

        private readonly IArticleService _articleService;

        private readonly ILogger<GatherService> _logger;

        public TimedExecutService(IOptionsSnapshot<SiteConfig> siteConfigOptions, IOptionsSnapshot<GatherConfig> gatherConfigOptions, IArticleService articleService, ILogger<GatherService> logger)
        {
            _siteConfig = siteConfigOptions.Value;
            _gatherConfig = gatherConfigOptions.Value;
            _articleService = articleService;
            _logger = logger;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    try
        //    {
        //        _logger.LogInformation(DateTime.Now.ToString() + "定时执行服务：启动");
        //        bool isRunning = false;
        //        DateTime lastRunTime;

        //        while (!stoppingToken.IsCancellationRequested && !isRunning)
        //        {
        //            await Task.Delay(60000, stoppingToken); //启动后第分钟执行一次扫描
        //            DateTime dt = DateTime.Now;
        //            if (_gatherConfig.TimingExecutionTasksOfWeek != null)
        //            {
        //                if (_gatherConfig.TimingExecutionTimeOfDay != null)
        //                {
        //                    foreach(var t in _gatherConfig.TimingExecutionTimeOfDay)
        //                    {
        //                        DateTime.Compare(dt, t);
        //                    }
        //                }
        //                else
        //                {

        //                }
        //            }                    
        //            _logger.LogInformation(DateTime.Now.ToString() + " 执行逻辑");
        //        }

        //        _logger.LogInformation(DateTime.Now.ToString() + "定时执行服务：停止");
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!stoppingToken.IsCancellationRequested)
        //        {
        //            _logger.LogInformation(DateTime.Now.ToString() + "定时执行服务：异常" + ex.Message + ex.StackTrace);
        //        }
        //        else
        //        {
        //            _logger.LogInformation(DateTime.Now.ToString() + "定时执行服务：停止");
        //        }
        //    }
        //}
    }
}
