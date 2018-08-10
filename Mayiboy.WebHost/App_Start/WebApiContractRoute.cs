using System.Web.Http;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.MvcExtensions.Routes;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Contract.Contracts;
using Mayiboy.Utils;

namespace Mayiboy.WebHost
{
	public class WebApiContractRoute
	{
		private static readonly object Queryappidauth = new object();

		/// <summary>
		/// 绑定路由
		/// </summary>
		/// <param name="config"></param>
		public static void Bind(HttpConfiguration config)
		{
			var entryRoute = new ApiContractRoute();

			//entryRoute.ExpiresTime = 10000;
			//entryRoute.IsBind = true;

			entryRoute.OnDecryptParameter = QueryDecryptParameter;

			//用户信息接口


			config.Routes.Add("singleEntryRoute", entryRoute);
		}

		/// <summary>
		/// 获取解密参数
		/// </summary>
		/// <param name="appid">应用标识</param>
		/// <param name="authtoken">授权Token</param>
		/// <param name="encryptionType">加密类型（0：不加密；1：对称加密（DES）；2：对称加密（AES）；3：非对称加密</param>
		/// <param name="secretKey">解密秘钥</param>
		private static void QueryDecryptParameter(string appid, ref string authtoken, ref int encryptionType, ref string secretKey)
		{
			if (!string.IsNullOrEmpty(appid))
			{
				var key = appid.AddCachePrefix("AppIdAuth");

				var entity = CacheManager.Get<AppIdAuthDto>(key, 60);

				if (entity == null)
				{
					lock (Queryappidauth)
					{
						entity = CacheManager.Get<AppIdAuthDto>(key, 60);

						if (entity == null)
						{
							var response = ServiceLocater.GetService<IAppIdAuthService>().QueryByAppId(new QueryByAppIdRequest { ServiceAppId = appid });

							if (response.IsSuccess && response.Entity != null)
							{
								entity = response.Entity;

								CacheManager.RedisDefault.Set(key, entity, PublicConst.Time.Hour2);
							}
						}
					}
				}

				if (entity != null)
				{
					authtoken = entity.AuthToken;
					encryptionType = entity.EncryptionType;

					switch (entity.EncryptionType)
					{
						case 1:
						case 2:
							secretKey = entity.SecretKey;
							break;
						case 3:
							secretKey = entity.PrivateKey;
							break;
					}
				}
			}
		}

	}
}