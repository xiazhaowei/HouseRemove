using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HouseRemove.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IList<string> lists, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {            
            return ListControlUtil.GenerateHtml(name, lists, repeatDirection, "radio", null);
        }


        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IList<string> lists, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {            
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return ListControlUtil.GenerateHtml(fullHtmlFieldName, lists, repeatDirection, "radio", metadata.Model);
        }





        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IList<string> lists, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {            
            return ListControlUtil.GenerateHtml(name, lists, repeatDirection, "checkbox", null);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IList<string> lists, RepeatDirection repeatDirection = RepeatDirection.Horizontal)
        {           
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            return ListControlUtil.GenerateHtml(fullHtmlFieldName, lists, repeatDirection, "checkbox", metadata.Model);
        }
    }
}