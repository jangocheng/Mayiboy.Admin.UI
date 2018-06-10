using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Host
{
    /// <summary>
    /// API 契约 路由
    /// </summary>
    public class ApiContractRoute : HttpRoute
    {
        #region ==== Fields

        private readonly bool _traceRoutes;//是否在运行时输出路由信息至输出面板中

        private Type _lastControllerType;//契约类型

        private readonly Hashtable _hashRoute = new Hashtable();//契约路由表【key：契约名；value：】

        #endregion

        #region ==== Ctors

        /// <summary>
        /// 构建APIContractRoutes新的实例
        /// </summary>
        /// <param name="traceRoutes">是否在运行时输出路由信息至输出面板中</param>
        public ApiContractRoute(bool traceRoutes = false)
        {
            _traceRoutes = traceRoutes;
        }

        #endregion

        #region ==== Public

        /// <summary>
        /// 绑定控制器
        /// </summary>
        /// <typeparam name="TController">Api控制器</typeparam>
        /// <returns>Api契约路由</returns>
        public ApiContractRoute Bind<TController>()
            where TController : ApiController
        {
            _lastControllerType = typeof(TController);

            return this;
        }

        /// <summary>
        /// 注册契约到通过Bind选择的控制器上
        /// </summary>
        /// <typeparam name="TContract">契约</typeparam>
        /// <returns>Api契约路由</returns>
        public ApiContractRoute With<TContract>()
            where TContract : IContract
        {
            if (_lastControllerType == null) { throw new ArgumentNullException("you must be invoke Bind<TController> func before this method"); }

            BindAction(typeof(TContract), _lastControllerType);

            return this;
        }

        #endregion

        #region ==== Private

        /// <summary>
        /// 执行注册契约到通过Bind选择的控制器上
        /// </summary>
        /// <param name="contractType">契约类型</param>
        /// <param name="controllerType">Api控制器</param>
        private void BindAction(Type contractType, Type controllerType)
        {
            var action = controllerType
               .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
               .FirstOrDefault(b => b.ReturnType == contractType.BaseType.GenericTypeArguments[1] && b.Name == contractType.Name.Substring(0, contractType.Name.Length - 8));

            if (action == null)
            {
                throw new ArgumentOutOfRangeException(string.Format("can't found action for contract of \"{0}\" in controller \"{1}\"", contractType, controllerType));
            }

            _hashRoute.Add(contractType.FullName, new Tuple<string, string>(controllerType.Name.Substring(0, controllerType.Name.Length - 10), action.Name));
        }

        #endregion

        #region ==== Override

        /// <summary>
        /// 获取路由信息
        /// </summary>
        /// <param name="virtualPathRoot">虚拟路径的跟</param>
        /// <param name="request">请求消息</param>
        /// <returns></returns>
        public override IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;

            var headers = context.Request.Headers;

            var contractHead = request.Headers.FirstOrDefault(b => b.Key == "ContractInfo");

            if (contractHead.Value == null || string.IsNullOrEmpty(contractHead.Value.FirstOrDefault()))
            {
                if (_traceRoutes)
                {
                    Trace.WriteLine("APIContractRoute : the contract headers is empty");
                }

                return base.GetRouteData(virtualPathRoot, request);
            }

            var contractFullName = contractHead.Value.FirstOrDefault();

            if (!_hashRoute.ContainsKey(contractFullName))
            {
                if (_traceRoutes)
                {
                    Trace.WriteLine(string.Format("APIContractRoute : the contract of \"{0}\" havn't bind", contractFullName));
                }

                return base.GetRouteData(virtualPathRoot, request);
            }

            var routeData = new HttpRouteData(this);

            var item = _hashRoute[contractFullName] as Tuple<string, string>;

            routeData.Values.Add("action", item.Item2);

            routeData.Values.Add("controller", item.Item1);

            if (_traceRoutes)
            {
                Trace.WriteLine(string.Format("APIContractRoute : contract = \"{0}\" , controller = \"{1}\" , action = \"{2}\"", contractFullName, item.Item1, item.Item2));
            }

            return routeData;
        }

        #endregion
    }
}