using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTestWeb2017.Service
{
    public class HomeService
    {
        public static void testhttpcontext(HttpContextBase context)
        {
            context.Items.Add("xx", "yy");
            context.Items.Add("x", "y");
            context.Response.Write(context.Items["x"]);
        }

        
    }
}