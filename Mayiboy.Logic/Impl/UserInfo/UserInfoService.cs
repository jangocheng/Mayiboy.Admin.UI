using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.DataAccess.Repository;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Logic.Impl
{
    /// <summary>
    /// 用户信息服务
    /// </summary>
    public class UserInfoService : BaseService, IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }


        public InsertResponse Insert(InsertRequest request)
        {
            var response = new InsertResponse();

            var entity = new UserInfoPo()
            {
                LoginName = "Simon",
                Password = "123456",
                Name = "蔡尚猛",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsValid = 1
            };

            entity = _userInfoRepository.InsertReturnEntity(entity);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LoginQueryResponse LoginQuery(LoginQueryRequest request)
        {
            var response = new LoginQueryResponse();
            try
            {
                var entity = _userInfoRepository.Find<UserInfoPo>(
                    e => e.IsValid == 1 && e.LoginName == request.LoginName && e.Password == request.Password);

                response.Content = entity.ToJson();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("登录查询出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}