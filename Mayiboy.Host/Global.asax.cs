using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Logging;
using Mayiboy.Host.Exception;
using Mayiboy.Logic.Mapper;

namespace Mayiboy.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //初始化日志组件
            LoggerGlobal.GlobalInit();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoMapperConfig.Configure();

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthorizationHandler());

            RegisterAndResolverIoc();
        }

        private void RegisterAndResolverIoc()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            TypeFinder typeFinder = new TypeFinder();

            foreach (var item in typeFinder.Assemblies)
            {
                DependencyConfig.RegisterDependency(item, builder);
            }

            var container = builder.Build();

            Framework.Mayiboy.Ioc.DependencyResolver.SetResolver(container);

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }

    }
}
