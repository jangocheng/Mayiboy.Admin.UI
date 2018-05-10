using System;
using System.Linq;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Logic.Impl
{
    public class PermissionsService : BaseService, IPermissionsService
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IRolePermissionsJoinRepository _rolePermissionsJoinRepository;

        public PermissionsService(IPermissionsRepository permissionsRepository, IRolePermissionsJoinRepository rolePermissionsJoinRepository)
        {
            _permissionsRepository = permissionsRepository;
            _rolePermissionsJoinRepository = rolePermissionsJoinRepository;
        }

        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllPermissionsResponse QueryAllPermissions(QueryAllPermissionsRequest request)
        {
            var response = new QueryAllPermissionsResponse();
            try
            {
                var list = _permissionsRepository.FindWhere<PermissionsPo>(e => e.IsValid == 1);

                response.EntityList = list.Select(e => e.As<PermissionsDto>()).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("查询所有权限：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryPermissionsResponse QueryPermissions(QueryPermissionsRequest request)
        {
            var response = new QueryPermissionsResponse();
            try
            {
                var list = _permissionsRepository.FindWhere<PermissionsPo>(e => e.IsValid == 1 && e.MenuId == request.MenuId);

                if (list != null)
                {
                    response.EntityList = list.Select(e => e.As<PermissionsDto>()).ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("查询权限出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 根据用户id查询用户权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryPermissionsByUserIdResponse QueryPermissionsByUserId(QueryPermissionsByUserIdRequest request)
        {
            var response = new QueryPermissionsByUserIdResponse();
            try
            {
                var list = _permissionsRepository.UseStoredProcedure<PermissionsPo>("proc_PermissionByUserId_select", new { UserId = request.UserId });

                if (list != null)
                {
                    response.EntityList = list.Select(e => e.As<PermissionsDto>()).ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("查询用户权限出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SavePermissionsResponse SavePermissions(SavePermissionsRequest request)
        {
            var response = new SavePermissionsResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "权限信息不能为空";
                return response;
            }

            try
            {
                var entity = request.Entity.As<PermissionsPo>();

                if (entity.Id == 0)
                {
                    //新增权限
                    EntityLogger.CreateEntity(entity);

                    _permissionsRepository.InsertReturnIdentity(entity);
                }
                else
                {
                    #region 更新权限信息
                    var entitytemp = _permissionsRepository.FindSingle<PermissionsPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新权限信息不存在");
                    }

                    EntityLogger.UpdateEntity(entity);

                    _permissionsRepository.UpdateIgnoreColumns(entity, e => new
                    {
                        e.IsValid,
                        e.CreateTime,
                        e.CreateUserId
                    });
                    #endregion
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("保存权限出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelPermissionsResponse DelPermissions(DelPermissionsRequest request)
        {
            var response = new DelPermissionsResponse();
            try
            {
                var entity = _permissionsRepository.FindSingle<PermissionsPo>(request.Id);

                if (entity == null)
                {
                    throw new Exception("删除权限不存在");
                }

                entity.IsValid = 0;

                EntityLogger.UpdateEntity(entity);

                _permissionsRepository.UpdateColumns(entity, e => new
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

                LogManager.LogicLogger.ErrorFormat("删除权限出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}