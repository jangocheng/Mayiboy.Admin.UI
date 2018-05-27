using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.Mayiboy.Ioc;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;

namespace Mayiboy.Host.Controllers
{
    /// <summary>
    /// 获取用户信息接口
    /// </summary>
    public class UserInfoController : ApiController
    {
        private readonly IUserInfoService _userinfoService;// = ServiceLocater.GetService<IUserInfoService>();

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userinfoService = userInfoService;
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public LoginQueryResponse LoginQuery(LoginQueryRequest request)
        {
            return _userinfoService.LoginQuery(request);
        }
    }
}
