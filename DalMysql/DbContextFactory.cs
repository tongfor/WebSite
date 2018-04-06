/** 
* DbContextFactory.cs
*
* 功 能： 数据层上下文接口MYSQL实现
* 类 名： DbContextFactory
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/8/29 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using IDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using System.IO;

namespace DALMySql
{
    public class DbContextFactory : IDbContextFactory
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 存取DbContext
        /// </summary>
        /// <returns></returns>
        public CdyhcdDBContext GetDbContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            DbContextOptions options = new DbContextOptionsBuilder()
                .UseMySQL(Configuration.GetConnectionString("YhcdConnection")).Options;

            CdyhcdDBContext dbContext = new CdyhcdDBContext(options);           

            return dbContext;
        }
    }
}
