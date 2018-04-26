using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using System.Linq;
using Mayiboy.Model.Dto;
using SqlSugar;

namespace Mayiboy.Logic.Impl
{
    public class UserRoleService : BaseService, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 分页查询用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryUserRoleResponse QueryUserRole(QueryUserRoleRequest request)
        {
            var response = new QueryUserRoleResponse();
            try
            {
                int total = 0;

                var list = _userRoleRepository.FindPage<UserRolePo>(
                    e => e.IsValid == 1 && (SqlFunc.IsNullOrEmpty(request.Name) || e.Name.Contains(request.Name)),
                    o => o.Id, request.PageIndex, request.PageSize, ref total, OrderByType.Desc);

                response.List = list.Select(e => e.As<UserRoleDto>()).ToList();

                response.TotalCount = total;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("分页查询角色出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserRoleResponse SaveUserRole(SaveUserRoleRequest request)
        {
            var response = new SaveUserRoleResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = "用户角色不能为空";
                return response;
            }

            try
            {
                var entity = request.Entity.As<UserRolePo>();

                if (entity.Id == 0)
                {
                    if (_userRoleRepository.Any<UserRolePo>(e => e.IsValid == 1 && e.Name == entity.Name))
                    {
                        response.IsSuccess = false;
                        response.MessageCode = "-1";
                        response.MessageText = "用户角色不能为空";
                        return response;
                    }

                    EntityLogger.CreateEntity(entity);
                    entity.Id = _userRoleRepository.InsertReturnIdentity(entity);
                }
                else
                {
                    var entitytemp = _userRoleRepository.FindSingle<UserRolePo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新用户角色信息不存在");
                    }

                    EntityLogger.UpdateEntity(entity);
                    _userRoleRepository.UpdateIgnoreColumns(entity, e => new { e.IsValid, e.CreateTime, e.CreateUserId, });
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("保存用户角色出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelUserRoleResponse DelUserRole(DelUserRoleRequest request)
        {
            var response = new DelUserRoleResponse();
            try
            {
                var entity = _userRoleRepository.FindSingle<UserRolePo>(request.Id);

                if (entity == null)
                {
                    throw new Exception("删除用户角色出错");
                }

                EntityLogger.UpdateEntity(entity);
                entity.IsValid = 0;

                _userRoleRepository.UpdateColumns(entity, e => new
                {
                    e.IsValid,
                    e.UpdateTime,
                    e.UpdateUserId
                });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("删除用户角色出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}