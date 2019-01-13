using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// 采集结果
    /// </summary>
    public class GatherResult
    {
        /// <summary>
        /// 站点关键字
        /// </summary>
        public string SiteKey { get; set; }

        /// <summary>
        /// 准备采集的文章清单
        /// </summary>
        public List<Article> PreGatherList { get; set; }

        /// <summary>
        /// 采集成功的文章清单
        /// </summary>
        public List<Article> GatheredArticleLIst { get; set; }
    }
}
