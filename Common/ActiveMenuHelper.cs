using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace Common
{
    public static class ActiveMenuHelper
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValue)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var currentArea = htmlHelper.ViewContext.RouteData.DataTokens["area"];

            var builder = new TagBuilder("a");
          
                //InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
                //InnerHtml = "<a href=\"" + new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName, new { area = areaName }).ToString() + "\">" + linkText + "</a>"
            builder.MergeAttribute("href", new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName, routeValue).ToString());
            builder.InnerHtml = linkText;
           
            if (String.Equals(controllerName, currentController, StringComparison.CurrentCultureIgnoreCase) && String.Equals(actionName, currentAction, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var currentArea = htmlHelper.ViewContext.RouteData.DataTokens["area"];

            var builder = new TagBuilder("a");

            //InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString()
            //InnerHtml = "<a href=\"" + new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName, new { area = areaName }).ToString() + "\">" + linkText + "</a>"
            builder.MergeAttribute("href", new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionName, controllerName).ToString());
            builder.InnerHtml = linkText;

            if (String.Equals(controllerName, currentController, StringComparison.CurrentCultureIgnoreCase) && String.Equals(actionName, currentAction, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }

    }
}
