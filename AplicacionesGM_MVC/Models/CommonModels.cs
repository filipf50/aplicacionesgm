using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc.Html;



public class DecimalModelBinder : DefaultModelBinder
{
    public override object BindModel(ControllerContext controllerContext,
                                     ModelBindingContext bindingContext)
    {
        object result = null;

        // Don't do this here!
        // It might do bindingContext.ModelState.AddModelError
        // and there is no RemoveModelError!
        // 
        // result = base.BindModel(controllerContext, bindingContext);

        string modelName = bindingContext.ModelName;
        string attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

        // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
        // Both "." and "," should be accepted, but aren't.
        string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        string alternateSeperator = (wantedSeperator == "," ? "." : ",");

        if (attemptedValue.IndexOf(wantedSeperator) == -1
            && attemptedValue.IndexOf(alternateSeperator) != -1)
        {
            attemptedValue =
                attemptedValue.Replace(alternateSeperator, wantedSeperator);
        }

        try
        {
            if (bindingContext.ModelMetadata.IsNullableValueType
                && string.IsNullOrWhiteSpace(attemptedValue))
            {
                return null;
            }

            result = decimal.Parse(attemptedValue, NumberStyles.Any);
        }
        catch (FormatException e)
        {
            result=base.BindModel(controllerContext, bindingContext);
        }

        return result;
    }
}

public static class CustomHelpers
{
    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
    {
        return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary <string, object> htmlAttributes)
    {
        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.')[1];
        if (String.IsNullOrEmpty(labelText))
        {
            return MvcHtmlString.Empty;
        }
        TagBuilder tag = new TagBuilder("label");
        tag.MergeAttributes(htmlAttributes);
        tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
        tag.SetInnerText(labelText);
        return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
    }

    public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression,bool blnAddValmsg)
    {
        string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        string htmlText=Convert.ToString(helper.ValidationMessageFor(expression));

        if (htmlText != "")
        {
            TagBuilder containerSpanBuilder = new TagBuilder("span");
            containerSpanBuilder.AddCssClass("field-validation-error");
            if (blnAddValmsg)
            {
                containerSpanBuilder.Attributes.Add("data-valmsg-for", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            }

            containerSpanBuilder.InnerHtml = htmlText.Replace("<span class=\"field-validation-error\">","").Replace("</span>","");

            return MvcHtmlString.Create(containerSpanBuilder.ToString(TagRenderMode.Normal));
        }
        else
        {
            return MvcHtmlString.Create("");
        }
    }

    public static MvcHtmlString ValidationMessage(this HtmlHelper helper, string fieldName, bool blnAddValmsg)
    {
        string htmlFieldName = fieldName;
        string htmlText = Convert.ToString(helper.ValidationMessage(fieldName));

        if (htmlText != "")
        {
            TagBuilder containerSpanBuilder = new TagBuilder("span");
            containerSpanBuilder.AddCssClass("field-validation-error");
            if (blnAddValmsg)
            {
                containerSpanBuilder.Attributes.Add("data-valmsg-for", helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            }

            containerSpanBuilder.InnerHtml = htmlText.Replace("<span class=\"field-validation-error\">", "").Replace("</span>", "");

            return MvcHtmlString.Create(containerSpanBuilder.ToString(TagRenderMode.Normal));
        }
        else
        {
            return MvcHtmlString.Create("");
        }
    }

    public static MvcHtmlString  TextBox(this HtmlHelper htmlHelper, string name, string id, object value, object htmlAttributes)
    {
        return TextBox(htmlHelper,name,id,value, new RouteValueDictionary(htmlAttributes));
    }

    public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name, string id, object value, IDictionary<string, object> htmlAttributes)
    {

        var tag = new TagBuilder("input");
        tag.MergeAttribute("type", "text");
        tag.MergeAttribute("name", name);
        tag.MergeAttribute("id", id);
        tag.MergeAttribute("value", Convert.ToString(value));
        tag.MergeAttributes(htmlAttributes);
        
        return  MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal)) ;
    }
}
