using System;
using System.Linq.Expressions;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.DataAccess.Repository;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using System.Linq;
using SqlSugar;

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
        /// 保存用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserInfoResponse SaveUserInfo(SaveUserInfoRequest request)
        {
            var response = new SaveUserInfoResponse();

            if (request.UserInfoEntity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "用户信息不能为空";
                return response;
            }

            try
            {
                var entity = request.UserInfoEntity.As<UserInfoPo>();

                if (entity.Id == 0)
                {
                    #region 新增
                    if (_userInfoRepository.Any<UserInfoPo>(e => e.IsValid == 1 && e.LoginName == entity.LoginName))
                    {
                        throw new Exception("账号已经存在");
                    }

                    if (_userInfoRepository.Any<UserInfoPo>(e => e.IsValid == 1 && e.Email == entity.Email))
                    {
                        throw new Exception("邮箱已经存在");
                    }

                    EntityLogger.CreateEntity(entity);

                    entity.Id = _userInfoRepository.InsertReturnIdentity(entity);
                    #endregion
                }
                else
                {
                    #region 更新
                    var entitytemp = _userInfoRepository.FindSingle<UserInfoPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新用户信息不存在");
                    }

                    EntityLogger.UpdateEntity(entity);

                    _userInfoRepository.UpdateIgnoreColumns(entity, e => new { e.IsValid, e.CreateTime, e.CreateUserId, e.HeadimgUrl });
                    #endregion
                }

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
        /// 查询用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryUserInfoResponse QueryUserInfo(QueryUserInfoRequest request)
        {
            var response = new QueryUserInfoResponse();
            try
            {
                int total = 0;

                //var list = _userInfoRepository.FindPage<UserInfoPo>(
                //    e => e.IsValid == 1
                //    && (SqlFunc.IsNullOrEmpty(request.Account) || e.LoginName.Contains(request.Account) || e.Email.Contains(request.Account))
                //    && (request.Sex == -1 || e.Sex == request.Sex),
                //    o => o.Id,
                //    request.PageIndex, request.PageSize, ref total, OrderByType.Desc);

                //response.EntityList = list.Select(e => e.As<UserInfoDto>()).ToList();

                response.EntityList = _userInfoRepository.QueryUserInfo(request.Account, request.Sex, request.DepartmentId, request.PageIndex, request.PageSize,
                    ref total);

                response.TotalCount = total;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("查询用户信息出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelUserInfoResponse Del(DelUserInfoRequest request)
        {
            var response = new DelUserInfoResponse();
            try
            {
                var entity = _userInfoRepository.Find<UserInfoPo>(e => e.IsValid == 1 && e.Id == request.Id);

                if (entity == null)
                {
                    throw new Exception("删除用户信息不存在");
                }

                entity.IsValid = 0;

                EntityLogger.UpdateEntity(entity);

                _userInfoRepository.UpdateColumns(entity, (e) => new { e.IsValid, e.UpdateTime, e.UpdateUserId });

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("删除用户信息：{0}", new { request, err = ex.ToString() }.ToJson());
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

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
        {
            var response = new ResetPasswordResponse();
            try
            {
                var entity = _userInfoRepository.Find<UserInfoPo>(e => e.IsValid == 1 && e.Id == request.UserId);

                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.MessageCode = "2";
                    response.MessageText = "用户不存在";
                    return response;
                }

                entity.Password = request.NewPassword.GetMd5();

                EntityLogger.UpdateEntity(entity);

                _userInfoRepository.UpdateColumns(entity, e => new
                {
                    e.Password,
                    e.UpdateUserId,
                    e.UpdateTime
                });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("重置密码出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            var response = new ChangePasswordResponse();

            if (request.OldPassword.IsNullOrEmpty())
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "旧密码不能为空";
                return response;
            }

            if (request.NewPassword.IsNullOrEmpty())
            {
                response.IsSuccess = false;
                response.MessageCode = "2";
                response.MessageText = "新密码不能为空";
                return response;
            }

            try
            {
                var entity = _userInfoRepository.Find<UserInfoPo>(e => e.IsValid == 1 && e.Id == request.UserId);

                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.MessageCode = "2";
                    response.MessageText = "用户不存在";
                    return response;
                }

                if (entity.Password != request.OldPassword.GetMd5())
                {
                    response.IsSuccess = false;
                    response.MessageCode = "3";
                    response.MessageText = "旧密码有误";
                    return response;
                }

                entity.Password = request.NewPassword.GetMd5();

                EntityLogger.UpdateEntity(entity);

                _userInfoRepository.UpdateColumns(entity, e => new
                {
                    e.Password,
                    e.UpdateUserId,
                    e.UpdateTime
                });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("更改密码出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}