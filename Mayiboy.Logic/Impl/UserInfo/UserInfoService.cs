using System;
using System.Linq.Expressions;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.DataAccess.Repository;
using Mayiboy.Model.Dto;
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

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InsertResponse Insert(InsertRequest request)
        {
            var response = new InsertResponse();

            try
            {
                var entity = request.UserInfoEntity.As<UserInfoPo>();

                entity = _userInfoRepository.InsertReturnEntity(entity);

                response.UserInfoEntity = entity.As<UserInfoDto>();
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

        /// <summary>
        /// 登录查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public LoginQueryResponse LoginQuery(LoginQueryRequest request)
        {
            var response = new LoginQueryResponse();
            try
            {
                var entity = _userInfoRepository.Find<UserInfoPo>(
                        e => e.IsValid == 1
                        && (e.LoginName == request.LoginName || e.Email == request.LoginName)
                        && e.Password == request.Password);

                response.UserInfoEntity = entity.As<UserInfoDto>();
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