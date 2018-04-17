using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Utils;

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
        /// 查询所有导航栏下的菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllMenuResponse QueryAllMenu(QueryAllMenuRequest request)
        {
            var response = new QueryAllMenuResponse();
            try
            {

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
    }
}