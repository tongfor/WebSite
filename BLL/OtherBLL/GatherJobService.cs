/** 
* GatherJobService.cs
*
* 功 能： 定时执行采集任务
* 类 名： GatherJobService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/2/20 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Common.Config;
using IBLL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomelo.AspNetCore.TimedJob;
//using Microsoft.Extensions.Hosting;

namespace BLL
{
    public class GatherJobService : Job
    {
        /// <summary>
        /// 本站设置
        /// </summary>
        private readonly SiteConfig _siteConfig;

        private readonly GatherConfig _gatherConfig;

        private readonly IGatherService _gatherServicee;

        private readonly ILogger<GatherService> _logger;

        public GatherJobService(IOptionsSnapshot<SiteConfig> siteConfigOptions, IOptionsSnapshot<GatherConfig> gatherConfigOptions, IGatherService gatherService, ILogger<GatherService> logger)
        {
            _siteConfig = siteConfigOptions.Value;
            _gatherConfig = gatherConfigOptions.Value;
            _gatherServicee = gatherService;
            _logger = logger;
        }        

        /// <summary>
        /// 定时执行任务
        /// </summary>
        // Begin 起始时间；Interval执行时间间隔，单位是毫秒，建议使用以下格式，此处为3小时；SkipWhileExecuting是否等待上一个执行完成，true为等待；
        [Invoke(Begin = "2019-2-20 16:20", Interval = 1000 * 3600 * 24, SkipWhileExecuting = true)]
        public async System.Threading.Tasks.Task RunAsync()
        {
            //Job要执行的逻辑代码

            _logger.LogInformation("Start timing gathering");
            await _gatherServicee.GatherAllWebsites(1, 3, 0, "adminauto");
            _logger.LogInformation("Finish timing gathering");
        }

    }
}
