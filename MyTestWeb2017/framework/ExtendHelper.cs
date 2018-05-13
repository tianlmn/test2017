using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWeb2017.framework
{
    public static class ExtendHelper
    {
        public static int ToInt(this string s)
        {
            int i;
            int.TryParse(s, out  i);
            return i;
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime dt;
            DateTime.TryParse(value, out dt);
            return dt;
        }
    }
}