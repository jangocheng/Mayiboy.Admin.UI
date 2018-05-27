using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Mayiboy.Ioc;
using Mayiboy.Utils;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;

namespace Mayiboy.Host.Exception
{
    public class AuthorizationHandler : DelegatingHandler
    {

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var headerappid = request.Headers.FirstOrDefault(e => e.Key.ToLower() == "appid");
            var headerauth = request.Headers.FirstOrDefault(e => e.Key.ToLower() == "authorizationtoken");

            if (string.IsNullOrEmpty(headerappid.Key) || string.IsNullOrEmpty(headerauth.Key))
            {
                return Unauthorized();
            }

            //获取appid token
            var appid = headerappid.Value.FirstOrDefault();
            var auth = headerauth.Value.FirstOrDefault();

            if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(auth))
            {
                return Unauthorized();
            }

            if (GetAppIdToken(appid) != headerauth.Value.FirstOrDefault())
            {
                return Unauthorized();
            }

            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 非法访问
        /// </summary>
        /// <returns></returns>
        private Task<HttpResponseMessage> Unauthorized()
        {
            //如果没有授权则返回403错误
            return Task.Factory.StartNew<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.Forbidden);
            });
        }

        /// <summary>
        /// 获取应用标识下的token
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        private string GetAppIdToken(string appid)
        {
            if (string.IsNullOrEmpty(appid)) return "";

            var key = appid.AddCachePrefix("ServiceToken");

            var token = CacheManager.Get(key, 60);

            if (token.IsNullOrEmpty())
            {
                var response = ServiceLocater.GetService<IAppIdAuthService>().QueryByAppId(new QueryByAppIdRequest
                {
                    ServiceAppId = appid
                });

                if (response.IsSuccess && response.Entity != null)
                {
                    token = response.Entity.AuthToken;

                    if (token.IsNotNullOrEmpty())
                    {
                        CacheManager.RedisDefault.Set(key, token, PublicConst.Time.Hour2);
                    }
                }
            }

            return token;
        }
    }
}