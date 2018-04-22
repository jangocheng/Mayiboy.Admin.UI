﻿using System;
using System.Linq.Expressions;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.DataAccess.Repository;
using Mayiboy.Model.Dto;
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
                    EntityLogger.CreateEntity(entity);

                    entity.Id = _userInfoRepository.InsertReturnIdentity(entity);
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

                    _userInfoRepository.UpdateIgnoreColumns(entity, e => new { e.IsValid, e.CreateTime, e.CreateUserId });
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

                var list = _userInfoRepository.FindPage<UserInfoPo>(
                    e => e.IsValid == 1
                    && (e.Name.Contains(request.Name) || SqlFunc.IsNullOrEmpty(request.Name)),
                    o => o.Id,
                    request.PageIndex, request.PageSize, ref total, OrderByType.Desc);

                response.EntityList = list.Select(e => e.As<UserInfoDto>()).ToList();

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
                var entity = _userInfoRepository.FindSingle<UserInfoPo>(request.Id);

                if (entity == null)
                {
                    throw new Exception("删除用户信息不存在");
                }

                entity.IsValid = 0;

                EntityLogger.UpdateEntity(entity);

                _userInfoRepository.UpdateColumns(entity, (e) => new {e.IsValid, e.UpdateTime, e.UpdateUserId});

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
    }
}