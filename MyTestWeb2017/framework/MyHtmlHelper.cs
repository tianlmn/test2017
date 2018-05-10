using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyTestWeb2017.framework
{
    public static class MyHtmlHelper
    {
        public const string lll = "<p style='color:red;'>hello</p>";

        public static string GroupPageRaw(this HtmlHelper helper)
        {
            string html = lll;
            return html;

        }


        public static MvcHtmlString GroupPageMVCHtml(this HtmlHelper helper)
        {
            string html1 = lll;
            return new MvcHtmlString(html1);

        }
    }
}