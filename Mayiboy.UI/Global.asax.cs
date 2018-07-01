using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Logging;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;
using Mayiboy.Logic.Mapper;
using Mayiboy.Utils;

namespace Mayiboy.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggerGlobal.GlobalInit();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutoMapperConfig.Initialize();

            RegisterAndResolverIoc();
        }

        #region 移除不必要的请求头
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            MvcHandler.DisableMvcResponseHeader = true;//移除X-AspNetMvc-Version HTTP头

            InitRsa();
        }
        #endregion

        #region 注册容器
        /// <summary>
        /// 注册容器
        /// </summary>
        private void RegisterAndResolverIoc()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //注册mvc容器的实现
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//没有为该对象定义无参数的构造函数。(mvc控制器构造函数注入)

            TypeFinder typeFinder = new TypeFinder();

            foreach (var item in typeFinder.Assemblies)
            {
                DependencyConfig.RegisterDependency(item, builder);
            }

            var container = builder.Build();

            Framework.Mayiboy.Ioc.DependencyResolver.SetResolver(container);
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
        #endregion

        #region 初始化 js加密参数
        /// <summary>
        /// 初始化 js加密参数
        /// </summary>
        private void InitRsa()
        {
            string exponent = CacheManager.RunTimeCache.Get("exponent");
            string modulus = CacheManager.RunTimeCache.Get("modulus");

            if (exponent.IsNullOrEmpty() || modulus.IsNullOrEmpty())
            {
                RsaCryption.JsPublicKey(PublicConst.XmlPrivateKey, out exponent, out modulus);

                CacheManager.RunTimeCache.Set("exponent", exponent, PublicConst.Time.Hour4);
                CacheManager.RunTimeCache.Set("modulus", modulus, PublicConst.Time.Hour4);
            }

            CookieHelper.Set("exponent", exponent);
            CookieHelper.Set("modulus", modulus);
        }
        #endregion
    }
}
