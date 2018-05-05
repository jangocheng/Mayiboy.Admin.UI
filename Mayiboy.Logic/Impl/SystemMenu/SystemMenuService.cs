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
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class SystemMenuService : BaseService, ISystemMenuService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISystemMenuRepository _systemMenuRepository;
        private readonly ISystemNavbarRepository _systemNavbarRepository;

        public SystemMenuService(ISystemMenuRepository systemMenuRepository,
            IUserInfoRepository userInfoRepository,
            ISystemNavbarRepository systemNavbarRepository)
        {
            _systemMenuRepository = systemMenuRepository;
            _userInfoRepository = userInfoRepository;
            _systemNavbarRepository = systemNavbarRepository;
        }

        /// <summary>
        /// 查询所有系统菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllSystemMenuResponse QueryAllSystemMenu(QueryAllSystemMenuRequest request)
        {
            var response = new QueryAllSystemMenuResponse();
            try
            {
                var list = _systemMenuRepository.FindWhere<SystemMenuPo>(e => e.IsValid == 1);

                response.SystemMenuList = list.Select(e => e.As<SystemMenuDto>()).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("查询所有系统菜单：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 查询所有导航栏下的菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllMenuResponse QueryAllMenu(QueryAllMenuRequest request)
        {
            var response = new QueryAllMenuResponse();
            try
            {
                var sugarparameter = new
                {
                    request.NavbarId
                };

                var list = _systemMenuRepository.UseStoredProcedure<SystemMenuPo>("proc_SystemMenuByNavbarId_select", sugarparameter);

                if (list != null)
                {
                    response.SystemMenuList = list.Select(e => e.As<SystemMenuDto>()).ToList();
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("查询导航栏下的菜单出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 根据用户id查询系统菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryMenuByUserIdResponse QueryMenuByUserId(QueryMenuByUserIdRequest request)
        {
            var response = new QueryMenuByUserIdResponse();
            try
            {

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("根据用户id查询系统菜单出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存系统菜单
        /// </summary>
        /// <param name="request">保存系统参数</param>
        /// <returns></returns>
        public SaveSystemMenuResponse SaveSystemMenu(SaveSystemMenuRequest request)
        {
            var response = new SaveSystemMenuResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = "系统菜单参数不能为空";
            }

            try
            {
                var entity = request.Entity.As<SystemMenuPo>();

                if (entity.Id == 0)
                {
                    #region 新增
                    EntityLogger.CreateEntity(entity);

                    response.Id = _systemMenuRepository.InsertReturnIdentity<SystemMenuPo>(entity);
                    #endregion
                }
                else
                {
                    #region 更新
                    var entitytemp = _systemMenuRepository.FindSingle<SystemMenuPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新系统菜单不存在");
                    }

                    EntityLogger.UpdateEntity(entity);

                    _systemMenuRepository.UpdateIgnoreColumns(entity, e => new
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

                LogManager.LogicLogger.ErrorFormat("保存系统菜单出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除系统菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelSystemMenuResponse DelSystemMenu(DelSystemMenuRequest request)
        {
            var response = new DelSystemMenuResponse();
            try
            {
                var list = _systemMenuRepository.UseStoredProcedure<SystemMenuPo>("proc_SystemMenuById_select", new { Id = request.Id });

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var entity = item;
                        EntityLogger.UpdateEntity(entity);
                        entity.IsValid = 0;

                        _systemMenuRepository.UpdateColumns(entity, e => new
                        {
                            e.IsValid,
                            e.UpdateTime,
                            e.UpdateUserId
                        });
                    }
                }
                else
                {
                    throw new Exception("删除系统菜单不存在");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("删除系统菜单出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}