using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Config
{
    /// <summary>
    /// 网站系统参数设置
    /// </summary>
    public class SiteConfig
    {
        #region 属性

        /// <summary>
        /// 网站前台访问域
        /// </summary>
        public string DefaultDomainName { get; set; }
        /// <summary>
        /// 前台用户注册默认的角色
        /// </summary>
        public string AllowDefaultRegRoles { get; set; }
        /// <summary>
        /// 后台账号默认密码
        /// </summary>
        public string DefaultAdminPwd { get; set; }
        /// <summary>
        /// 后台文件上传文件夹
        /// </summary>
        public string DefaultUploadFolder { get; set; }
        /// <summary>
        /// 后台文件上传大小
        /// </summary>
        public string DefaultUploadFileMaxSize { get; set; }
        /// <summary>
        /// 商业计划书文件上传大小
        /// </summary>
        public string DefaultBusinessPlanUploadFileMaxSize { get; set; }
        /// <summary>
        /// 后台允许图片文件后缀
        /// </summary>
        public string AllowUploadImageFileExt { get; set; }
        /// <summary>
        /// 后台允许FLASH文件后缀
        /// </summary>
        public string AllowUploadFlashFileExt { get; set; }
        /// <summary>
        /// 后台允许多媒体文件后缀
        /// </summary>
        public string AllowUploadMediaFileExt { get; set; }
        /// <summary>
        /// 后台允许一般文件后缀
        /// </summary>
        public string AllowUploadFileExt { get; set; }
        /// <summary>
        /// 允许商业计划书后缀
        /// </summary>
        public string AllowBusinessPlanUploadFileExt { get; set; }
        /// <summary>
        /// 后台登录允许的角色ID
        /// </summary>
        public string AllowAdminRoles { get; set; }
        /// <summary>
        /// 前台登录允许的角色ID
        /// </summary>
        public string AllowStageRoles { get; set; }
        /// <summary>
        /// 评分专家角色ID
        /// </summary>
        public string ProfessorRoleId { get; set; }
        /// <summary>
        /// 申请入驻文档保存位置
        /// </summary>
        public string ApplyEnterWordPath { get; set; }
        /// <summary>
        /// 后台默认登录过期小时数
        /// </summary>
        public double DefaultLoginExpiresHours { get; set; } = 2;
        /// <summary>
        /// 默认记住账号天数
        /// </summary>
        public int DefaultRemberDays { get; set; } = 7;
        /// <summary>
        /// 同一IP允许登录失败最大限制
        /// </summary>
        public int MaxLoginErrorCount { get; set; } = 5;
        /// <summary>
        /// 同一IP登录失败超过最大限制后，多长时间能重新登录
        /// </summary>
        public int LoginErrorTryMinutes { get; set; } = 30;
        /// <summary>
        /// 政策信息栏目ID
        /// </summary>
        public int PolicyClassId { get; set; } = 2;
        /// <summary>
        /// 申报快讯栏目ID
        /// </summary>
        public int NotificationClassId { get; set; } = 3;
        /// <summary>
        /// 政策信息过滤关键字
        /// </summary>
        public string PolicyKeywords { get; set; } = "暂行办法,管理办法,实施细则,申报指南";
        /// <summary>
        /// 申报快讯过滤关键字
        /// </summary>
        public string NotificationKeywords { get; set; } = "项目 and 征集,申报";
        /// <summary>
        /// 采集站点属性
        /// </summary>
        public List<GatherWebsite> GatherWebsiteList { get; set; }

        #endregion
    }

    /// <summary>
    /// 采集站点属性
    /// </summary>
    public class GatherWebsite
    {
        /// <summary>
        /// 采集站点关键字
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 站点名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 站点域名
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// 采集URL模板
        /// </summary>
        public string UrlTemp { get; set; }
        /// <summary>
        /// 是否采集此文章是否由文章详情页中特性确定
        /// </summary>
        public bool IsGatherByDetail { get; set; }

    }
}
