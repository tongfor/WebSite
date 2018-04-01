using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Common;
namespace System.Web.Mvc.Html
{
    public static class CheckBoxExtensions
    {
        public static MvcHtmlString CheckBoxListForParamList<TModel, TProperty, ParModel>(this HtmlHelper<TModel> htmlHelper, Collection<ParModel> collections, Expression<Func<TModel, TProperty>> expression, RepeatDirection repeatDirection = RepeatDirection.Horizontal,int rowNumber=3)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return CheckBoxListForParmListUtil.GenerateHtml(fullHtmlFieldName, collections as Collection<Models.Parameter>, repeatDirection, "checkbox", metadata.Model, rowNumber);
        }
    }
}
