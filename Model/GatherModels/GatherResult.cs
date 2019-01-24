using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// 站点名
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 展示采集结果的网站域
        /// </summary>
        public string ResultShowDomainName { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        [DisplayName("采集时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime GatherTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 准备采集的文章清单
        /// </summary>
        public List<Article> PreGatherList { get; set; }

        /// <summary>
        /// 采集成功的文章清单
        /// </summary>
        public List<Article> GatheredArticleList { get; set; }
    }
}
