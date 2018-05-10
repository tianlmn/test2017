using System;
using System.Linq;
using System.Web;

namespace MyTestWeb2017.framework
{
    public static class RequestHelper
    {
        public static string GetStringFromParameters(string key)
        {
            if (HttpContext.Current == null)
                return string.Empty;
            var value = HttpContext.Current.Request[key];
            return !string.IsNullOrEmpty(value) ? value : string.Empty;
        }

        /// <summary>
        /// Get value from QueryString or Form data
        /// </summary>
        public static string GetStringFromQueryString(string key)
        {
            if (HttpContext.Current == null || string.IsNullOrEmpty(key))
                return string.Empty;
            var value = HttpContext.Current.Request.QueryString[key];
            if (!string.IsNullOrEmpty(value))
                return value;

            value = HttpContext.Current.Request.Form[key];
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        /// <summary>
        /// Get value from Cookies
        /// </summary>
        public static string GetStringFromCookie(string cookieName)
        {
            if (HttpContext.Current == null || string.IsNullOrEmpty(cookieName))
                return string.Empty;

            var cookie = HttpContext.Current.Request.Cookies[cookieName];

            return cookie == null ? string.Empty : cookie.Value;
        }

        /// <summary>
        /// Get value from Cookies
        /// </summary>
        public static string GetStringFromCookie(string cookieName, string key)
        {
            if (HttpContext.Current == null || string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(key))
                return string.Empty;

            var cookie = HttpContext.Current.Request.Cookies[cookieName];

            if (cookie == null)
                return string.Empty;

            return cookie.Values.AllKeys.Contains(key) ? cookie.Values[key] : string.Empty;
        }


        public static int GetIntFromParameters(string key)
        {
            return Convert.ToInt32(GetStringFromParameters(key));
        }

        public static int GetIntFromQueryString(string key)
        {
            return Convert.ToInt32(GetStringFromQueryString(key));
        }

        public static long GetLongIntFromParameters(string key)
        {
            return Convert.ToInt64(GetStringFromParameters(key));
        }

        public static decimal GetDecimalFromParameters(string key)
        {
            return Convert.ToDecimal(GetStringFromParameters(key));
        }

        public static bool GetBooleanFromParameters(string key)
        {
            return Convert.ToBoolean(GetStringFromParameters(key));
        }


        public static string Referrer => HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];

        public static string RawUrl => HttpContext.Current.Request.RawUrl;

        public static string QueryString => HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Url.Query;

        public static string Host => HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Url.Host;

        public static string LocalPath => HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.Url.LocalPath;
        
        public static string UserAgent => HttpContext.Current == null ? string.Empty : HttpContext.Current.Request.UserAgent;
    }
}
