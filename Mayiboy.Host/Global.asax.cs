using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Framework.Mayiboy.Logging;
using Mayiboy.Logic.Mapper;

namespace Mayiboy.Host
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggerGlobal.GlobalInit();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            AutoMapperConfig.Configure();
        }
    }
}
