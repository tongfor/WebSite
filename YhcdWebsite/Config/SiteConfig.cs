using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YhcdWebsite.Config
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
        /// 前台行业资讯栏目ID
        /// </summary>
        public string IndustryInformationClassIds { get; set; }
        /// <summary>
        /// 前台默认新闻每页显示条数
        /// </summary>
        public int DefaultPageCount { get; set; }
        #endregion
    }
}
