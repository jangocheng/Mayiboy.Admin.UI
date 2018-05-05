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
        private readonly IUserRoleRepository _userRoleRepository;//用户角色
        private readonly IUserRoleJoinRepository _userRoleJoinRepository;//用户角色关联
        private readonly IRolePermissionsJoinRepository _rolePermissionsJoinRepository;//角色权限关联

        public UserRoleService(IUserRoleRepository userRoleRepository, IUserRoleJoinRepository userRoleJoinRepository, IRolePermissionsJoinRepository rolePermissionsJoinRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRoleJoinRepository = userRoleJoinRepository;
            _rolePermissionsJoinRepository = rolePermissionsJoinRepository;
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

        /// <summary>
        /// 查询用户角色关联列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryUserRoleJoinResponse QueryUserRoleJoin(QueryUserRoleJoinRequest request)
        {
            var response = new QueryUserRoleJoinResponse();
            try
            {
                var list = _userRoleJoinRepository.FindWhere<UserRoleJoinPo>(e => e.IsValid == 1 && e.UserId == request.UserId);

                response.EntityList = list.Select(e => e.As<UserRoleJoinDto>()).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("查询用户角色关联列表出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存用户角色关联
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserRoleJoinResponse SaveUserRoleJoin(SaveUserRoleJoinRequest request)
        {
            var response = new SaveUserRoleJoinResponse();
            try
            {
                var olduserrolejoinlist = _userRoleJoinRepository.FindWhere<UserRoleJoinPo>(e => e.IsValid == 1 && e.UserId == request.UserId);

                #region 删除角色关联
                var deluserrolejoin = olduserrolejoinlist.Where(e => !request.RoleIdList.Contains(e.RoleId)).ToList();

                foreach (var item in deluserrolejoin)
                {
                    item.IsValid = 0;
                    EntityLogger.UpdateEntity(item);

                    _userRoleJoinRepository.UpdateColumns(item, e => new { e.IsValid, e.UpdateTime, e.UpdateUserId });
                }
                #endregion

                #region 新增角色关联
                var newuserrolejoinlist = request.RoleIdList.Where(e => olduserrolejoinlist.All(o => o.RoleId != e));

                foreach (var item in newuserrolejoinlist)
                {
                    var entity = new UserRoleJoinPo
                    {
                        UserId = request.UserId,
                        RoleId = item
                    };

                    EntityLogger.CreateEntity(entity);
                    _userRoleJoinRepository.Insert(entity);
                }
                #endregion

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("保存用户角色关联出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除用户角色关联
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelUserRoleJoinResponse DelUserRoleJoin(DelUserRoleJoinRequest request)
        {
            var response = new DelUserRoleJoinResponse();
            try
            {
                var entity = _userRoleJoinRepository.FindSingle<UserRoleJoinPo>(request.Id);

                if (entity == null)
                {
                    throw new Exception("删除用户角色关联不存在");
                }

                entity.IsValid = 0;
                EntityLogger.UpdateEntity(entity);
                _userRoleJoinRepository.UpdateColumns(entity, e => new
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
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("删除用户角色关联出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 查询角色菜单权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryRoleMenuPermissionsResponse QueryRoleMenuPermissions(QueryRoleMenuPermissionsRequest request)
        {
            var response = new QueryRoleMenuPermissionsResponse();
            try
            {
               var list=  _rolePermissionsJoinRepository.FindWhere<RolePermissionsJoinPo>(
                    e =>e.IsValid == 1 
                    && e.NavbarId == request.NavbarId 
                    && e.MenuId == request.MenuId 
                    && e.RoleId == request.RoleId);

                response.EntityList = list.Select(e => e.As<RolePermissionsJoinDto>()).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("查询角色菜单权限出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveRolePermissionsResponse SaveRolePermissions(SaveRolePermissionsRequest request)
        {
            var response = new SaveRolePermissionsResponse();
            try
            {
                var oldrolepjoinlist = _rolePermissionsJoinRepository.FindWhere<RolePermissionsJoinPo>(
                    e => e.IsValid == 1
                    && e.NavbarId == request.NavbarId
                    && e.MenuId == request.MenuId
                    && e.RoleId == request.RoleId);

                #region 删除角色权限
                var delrolepjoin = oldrolepjoinlist.Where(e => !request.PermissionsIds.Contains(e.PermissionsId)).ToList();
                foreach (var item in delrolepjoin)
                {
                    item.IsValid = 0;
                    EntityLogger.UpdateEntity(item);

                    _rolePermissionsJoinRepository.UpdateColumns(item, e => new { e.IsValid, e.UpdateTime, e.UpdateUserId });
                }
                #endregion

                #region 新增角色权限
                var newrolepjoinlist = request.PermissionsIds.Where(e => oldrolepjoinlist.All(o => o.PermissionsId != e));

                foreach (var item in newrolepjoinlist)
                {
                    var entity = new RolePermissionsJoinPo
                    {
                        NavbarId = request.NavbarId,
                        MenuId = request.MenuId,
                        RoleId = request.RoleId,
                        PermissionsId = item
                    };

                    EntityLogger.CreateEntity(entity);
                    _rolePermissionsJoinRepository.Insert(entity);
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;

                LogManager.LogicLogger.ErrorFormat("保存角色权限出错:{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}