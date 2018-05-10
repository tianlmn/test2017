using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Http;
using IBU.Hotel.Gateway.Common.Helpers;
using MyTestWeb2017.Models;

namespace MyTestWeb2017.Controllers
{
    public class WebApiController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetJson(HttpRequestMessage request)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            var model = new WebApiModel() {ipcontent = GetClientIp(request) };
            var result = JsonHelper.SerializeObject(model);
            response.Content = new StringContent(result, Encoding.Unicode, "application/json");
            return response;
        }


        private string GetClientIp(HttpRequestMessage request)
        {
            string remote = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string via = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            string forward = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string strHostName = System.Net.Dns.GetHostName();
            var addresses = System.Net.Dns.GetHostAddresses(strHostName);
            string clientIpAddress0 = addresses.GetValue(0).ToString();
            string clientIpAddressx = addresses.GetValue(addresses.Length - 1).ToString();
            var host = HttpContext.Current.Request.UserHostAddress;
            var ms = string.Empty;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                ms = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            var remoteend = string.Empty;
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                remoteend = prop.Address;
            }

            var ips = $"REMOTE_ADDR:{remote}|HTTP_VIA:{via}|HTTP_X_FORWARDED_FOR:{forward}|HostAddresses0:{clientIpAddress0}|HostAddressesx:{clientIpAddressx}|host:{host}|MS_HttpContext:{ms}|RemoteEndpoint:{remoteend}";

            return ips;
        }
    }
}
