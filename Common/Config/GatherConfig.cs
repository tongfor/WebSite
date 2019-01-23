using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Config
{
    /// <summary>
    /// 采集参数设置
    /// </summary>
    public class GatherConfig
    {
        #region 属性

        /// <summary>
        /// 政策信息过滤关键字
        /// </summary>
        public string PolicyKeywords { get; set; } = "暂行办法,管理办法,实施细则,申报指南";
        /// <summary>
        /// 申报快讯过滤关键字
        /// </summary>
        public string NotificationKeywords { get; set; } = "项目 and 征集,申报";
        /// <summary>
        /// 要排除的关键字
        /// </summary>
        public string ExcludeKeywords { get; set; } = "投标,比选,办理情况,公开表,诊所,审批,告知书,搬迁,房屋征收,通告,门诊部,公示表,许可证";
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
        /// 采集页第一页URL，为空则采用采集URL模板地址
        /// </summary>
        public string FirstPageUrl { get; set; }
        /// <summary>
        /// 文章列表选择特征
        /// </summary>
        public string ArticleListSelector { get; set; }       
        /// <summary>
        /// 文章列表中标题选择特征
        /// </summary>
        public string TitleSelectorInList { get; set; }
        /// <summary>
        /// 文章列表中标题取值属性
        /// </summary>
        public string TitleAttributeNameInList { get; set; }
        /// <summary>
        /// 文章详情外围块特征
        /// </summary>
        public string DetailsSelector { get; set; }
        /// <summary>
        /// 要采集的细节清单
        /// </summary>
        public List<GatherDetails> DetailsList { get; set; }
        public string OriginReplacer { get; set; }
        /// <summary>
        /// 是否采集此文章是否由文章详情页中特性确定
        /// </summary>
        public bool IsGatherByDetail { get; set; }
        /// <summary>
        /// 文章详细采集是否使用Javascript配置
        /// </summary>
        public bool DetailIsUseJavascript { get; set; }
    }

    public static class GatherWebsiteExtension
    {
        /// <summary>
        /// 得到细节选择特征
        /// </summary>
        /// <param name="gatherWebsite"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GatherDetails GetGatherDetailsByName(this GatherWebsite gatherWebsite, string name)
        {
            return gatherWebsite?.DetailsList?.FirstOrDefault(f => name.Equals(f.Name, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    /// <summary>
    /// 要采集的文章细节
    /// </summary>
    public class GatherDetails
    {
        /// <summary>
        /// 细节名，如Title,Content
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 选择特征
        /// </summary>
        public string Selector { get; set; }
        /// <summary>
        /// 从哪个属性中取值
        /// </summary>
        public string ValueAttributeName { get; set; }
        /// <summary>
        /// 根据特征选择的结果如果是多个，则从第几个结果中获取结果,0为选择第一个
        /// </summary>
        public int ValueOrder { get; set; }
        /// <summary>
        /// 取值前特征
        /// </summary>
        public string ValueForward { get; set; }
        /// <summary>
        /// 取值后特征
        /// </summary>
        public string ValueAfter { get; set; }
        /// <summary>
        /// 取值类型，1为取text,2为取innerHtml,3为取outerHtml,默认为1
        /// </summary>
        public ValueType ValueType { get; set; }
        /// <summary>
        /// 取值被替换部分，多个要被替换部分以“,”分隔
        /// </summary>
        public string BeReplacedStr { get; set; }
        /// <summary>
        /// 取值替换词，多个要替换词以“,”分隔，替换为空则不填
        /// </summary>
        public string Replacer { get; set; }
        /// <summary>
        /// 取值要删除的节点特征，多个要删除节点以“,”分隔
        /// </summary>
        public string RemoveSelector { get; set; }
    }
    
    /// <summary>
    /// 取值类型，1为取text,2为取innerHtml,3为取outerHtml,默认为1
    /// </summary>
    public enum ValueType
    {
        text = 1,
        innerHtml = 2,
        outerHtml = 3
    }
}
