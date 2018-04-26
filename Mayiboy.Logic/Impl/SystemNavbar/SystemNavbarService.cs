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
        /// 获取系统栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetResponse Get(GetRequest request)
        {
            var response = new GetResponse();
            try
            {
                var entity = _systemNavbarRepository.FindSingle<SystemNavbarPo>(request.Id);

                response.Entity = entity.As<SystemNavbarDto>();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("根据Id获取系统栏目出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
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
        /// 查询栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryResponse Query(QueryRequest request)
        {
            var response = new QueryResponse();
            try
            {
                int total = 0;

                var list = _systemNavbarRepository.FindPage<SystemNavbarPo>(
                    e => e.IsValid == 1 && (e.Name.Contains(request.Name) || SqlFunc.IsNullOrEmpty(request.Name)),
                    o => o.Sort,
                    request.PageIndex, request.PageSize, ref total);

                response.SystemNavbarList = list.Select(e => e.As<SystemNavbarDto>()).ToList();

                response.TotalCount = total;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("分页查询系统栏目出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        //保存系统栏目
        public SaveResponse Save(SaveRequest request)
        {
            var response = new SaveResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "系统栏目参数不能为空";
                return response;
            }

            try
            {
                var entity = request.Entity.As<SystemNavbarPo>();

                if (entity.Id == 0)
                {
                    #region 新增

                    EntityLogger.CreateEntity(entity);

                    response.Id = _systemNavbarRepository.InsertReturnIdentity<SystemNavbarPo>(entity);
                    #endregion
                }
                else
                {
                    #region 更新
                    var entitytemp = _systemNavbarRepository.FindSingle<SystemNavbarPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新系统栏目不存在");
                    }

                    EntityLogger.UpdateEntity(entity);

                    _systemNavbarRepository.UpdateIgnoreColumns(entity, e => new { e.IsValid, e.CreateTime, e.CreateUserId });
                    #endregion
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("保存系统栏目出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        //删除系统栏目
        public DelResponse Del(DelReqeust request)
        {
            var response = new DelResponse();
            try
            {
                var entity = _systemNavbarRepository.FindSingle<SystemNavbarPo>(request.Id);

                if (entity == null)
                {
                    throw new Exception("删除系统栏目不存在");
                }
                
                EntityLogger.UpdateEntity(entity);

                entity.IsValid = 0;

                _systemNavbarRepository.UpdateColumns<SystemNavbarPo>(entity, (e) => new { e.IsValid, e.UpdateTime, e.UpdateUserId });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("删除系统栏目出错：{0}", new { request, err = ex.ToString() }.ToJson());
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