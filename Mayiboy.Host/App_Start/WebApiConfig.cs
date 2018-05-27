using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Mayiboy.Contract.Contracts;
using Mayiboy.Host.Controllers;

namespace Mayiboy.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //绑定服务
            var entryRoute = new ApiContractRoute(true);


            //用户信息接口
            entryRoute.Bind<UserInfoController>()
              .With<LoginQueryContract>();


            config.Routes.Add("singleEntryRoute", entryRoute);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
