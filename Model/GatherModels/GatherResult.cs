﻿using System;
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
        /// 站点域名
        /// </summary>
        public string SiteUrl { get; set; }

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

        /// <summary>
        /// 结果说明
        /// </summary>
        public string GatherMessage { get; set; }

        /// <summary>
        /// 采集报错码
        /// </summary>
        public GatherErrorCode ErrorStatus { get; set; }
    }

    /// <summary>
    /// 操作结果类型定义枚举
    /// </summary>
    public enum GatherErrorCode
    {
        None = 0,
        Is404 = 1,
        Is502 = 2,
        Is500 = 3,
        Other = 4
    }
}
