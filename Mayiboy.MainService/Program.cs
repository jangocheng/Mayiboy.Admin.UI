using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Logging;
using Mayiboy.Logic.Mapper;

namespace Mayiboy.MainService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            LoggerGlobal.GlobalInit();
            RegisterAndResolverIoc();//注册服务
            AutoMapperConfig.Configure();

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);


            Application.Run(new Form1());
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        private static void RegisterAndResolverIoc()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            TypeFinder typeFinder = new TypeFinder();

            foreach (var item in typeFinder.Assemblies)
            {
                DependencyConfig.RegisterDependency(item, builder);
            }

            var container = builder.Build();

            DependencyResolver.SetResolver(container);

            //System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
