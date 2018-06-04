/** 
* PaginationViewComponent.cs
*
* 功 能： 分页控件（定义分页参数）
* 类 名： PaginationViewComponent
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/5/28 10:24:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryPattern;

namespace WebAdmin.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        protected readonly IHostingEnvironment Env;
        protected readonly CdyhcdDBContext BusinessDbContext;
        protected readonly ILogger Logger;

        public PaginationViewComponent(IHostingEnvironment env, CdyhcdDBContext context, ILoggerFactory loggerFactory)
        {
            Env = env;
            BusinessDbContext = context;
            Logger = loggerFactory.CreateLogger<PaginationViewComponent>();
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
