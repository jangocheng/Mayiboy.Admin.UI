using System;
using System.Linq;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using SqlSugar;

namespace Mayiboy.Logic.Impl
{
    public class SystemNavbarService : BaseService, ISystemNavbarService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISystemNavbarRepository _systemNavbarRepository;

        public SystemNavbarService(IUserInfoRepository userInfoRepository, ISystemNavbarRepository systemNavbarRepository)
        {
            _userInfoRepository = userInfoRepository;
            _systemNavbarRepository = systemNavbarRepository;
        }

        /// <summary>
        /// 查询所有系统导航
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllNavbarResponse QueryAll(QueryAllNavbarRequest request)
        {
            var response = new QueryAllNavbarResponse();
            try
            {
                var list = _systemNavbarRepository.FindWhere<SystemNavbarPo>(e => e.IsValid == 1, e => e.Sort);

                response.SystemNavbarList = list.Select(e => e.As<SystemNavbarDto>()).ToList();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("查询系统所有栏目出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 根据用户名Id查询系统导航栏列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryNavbarResponse QueryNavbar(QueryNavbarRequest request)
        {
            var response = new QueryNavbarResponse();
            try
            {
                //:TODO 待处理（根据用户名Id查询导航栏列表）
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("根据用户Id查询系统导航栏列表出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}