using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
			var headertoken = request.Headers.FirstOrDefault(e => e.Key.ToLower() == "authorizationtoken");
			var headertimestamp = request.Headers.FirstOrDefault(e => e.Key.ToLower() == "timestamp");
			var headersign = request.Headers.FirstOrDefault(e => e.Key.ToLower() == "sign");


			if (string.IsNullOrEmpty(headerappid.Key) || string.IsNullOrEmpty(headertoken.Key))
			{
				return Unauthorized();
			}

			//获取appid token timestamp sign
			var appid = headerappid.Value.FirstOrDefault();
			var token = headertoken.Value.FirstOrDefault();
			var timestamp = headertimestamp.Value.FirstOrDefault();
			var sign = headersign.Value.FirstOrDefault();
			var content = request.Content.ReadAsStringAsync().Result;

			//接口必要参数
			if (string.IsNullOrEmpty(appid) || string.IsNullOrEmpty(token))
			{
				return Unauthorized();
			}

			//校验授权
			if (GetAppIdToken(appid) != token)
			{
				return Unauthorized();
			}

			//校验接口有效期(接口10分钟后无效)
			if (string.IsNullOrEmpty(timestamp) || long.Parse(timestamp).ToDateTime() < DateTime.Now.AddMinutes(-10))
			{
				return Unauthorized();
			}

			//TODO:需要确认content密文or 明文

			//校验sign
			//var signtemp = GetSign(appid, token, content, timestamp);
			//if (!string.IsNullOrEmpty(sign) && sign != signtemp)
			//{
			//	return Unauthorized();
			//}

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

		/// <summary>
		/// 获取 Gign
		/// </summary>
		/// <param name="appid"></param>
		/// <param name="accessToken"></param>
		/// <param name="content"></param>
		/// <param name="timestamp"></param>
		/// <returns></returns>
		private string GetSign(string appid, string accessToken, string content, string timestamp)
		{
			StringBuilder sb = new StringBuilder();

			Dictionary<string, string> stringADict = new Dictionary<string, string>();

			stringADict.Add("appid", appid);
			stringADict.Add("authorizationtoken", accessToken);
			stringADict.Add("contentstring", content);
			stringADict.Add("time", timestamp);

			foreach (var item in stringADict.OrderBy(x => x.Key)) //参数名ASCII码从小到大排序（字典序）；
			{
				sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
			}

			var sign = sb.ToString().GetMd5().ToLower();//对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为小写，得到sign值signValue。 
			return sign;

		}
	}
}