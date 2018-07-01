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

namespace Mayiboy.Admin.UI
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			LoggerGlobal.GlobalInit();
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
			//没有为该对象定义无参数的构造函数。(mvc控制器构造函数注入)
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

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
			//TODO: 加锁防止重复写入缓存、判断客户端Cookies是否已经存在有效（现在不会导致程序出错，只是会重复写入到缓存内）

			string exponent = CacheManager.RunTimeCache.Get("exponent");
			string modulus = CacheManager.RunTimeCache.Get("modulus");

			if (exponent.IsNullOrEmpty() || modulus.IsNullOrEmpty())
			{
				RsaCryption.JsPublicKey(PublicConst.XmlPrivateKey, out exponent, out modulus);

				CacheManager.RunTimeCache.Set("exponent", exponent, PublicConst.Time.Day1);
				CacheManager.RunTimeCache.Set("modulus", modulus, PublicConst.Time.Day1);
			}

			//var clientexponent = CookieHelper.Get("exponent");
			//var clientmodulus = CookieHelper.Get("modulus");

			CookieHelper.Set("exponent", exponent);
			CookieHelper.Set("modulus", modulus);

			#region 判断是否需要写入到客户端
			//if (!(clientexponent.IsNotNullOrEmpty() && clientexponent == exponent))
			//{
				
			//}

			//if (!(clientmodulus.IsNotNullOrEmpty() && clientmodulus == modulus))
			//{
				
			//}
			#endregion

		}
		#endregion
	}
}
