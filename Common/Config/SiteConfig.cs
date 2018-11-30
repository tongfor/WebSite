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
        public string DefaultLoginExpiresHours { get; set; }
        #endregion
    }
}
