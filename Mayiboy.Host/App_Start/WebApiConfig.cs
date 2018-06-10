using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Framework.Mayiboy.MvcExtensions.Routes;
using Mayiboy.Contract.Contracts;
using Mayiboy.Host.Controllers;

namespace Mayiboy.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.MapHttpAttributeRoutes();

            //绑定服务
            WebApiContractRoute.Bind(config);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
