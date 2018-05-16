using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using System.Linq;

namespace Mayiboy.Logic.Impl
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserDepartmentJoinRepository _userDepartmentJoinRepository;

        public DepartmentService(IDepartmentRepository departmentRepository,
            IUserDepartmentJoinRepository userDepartmentJoinRepository)
        {
            _departmentRepository = departmentRepository;
            _userDepartmentJoinRepository = userDepartmentJoinRepository;
        }

        /// <summary>
        /// 查询所有部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAllDepartmentResponse QueryAllDepartment(QueryAllDepartmentRequest request)
        {
            var response = new QueryAllDepartmentResponse();
            try
            {
                var list = _departmentRepository.FindWhere<DepartmentPo>(e => e.IsValid == 1);

                response.List = list.Select(e => e.As<DepartmentDto>()).ToList();

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("查询部门出错:{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveDepartmentResponse SaveDeparment(SaveDepartmentRequest request)
        {
            var response = new SaveDepartmentResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "保存部门信息不存在";
                return response;
            }

            try
            {
                var entity = request.Entity.As<DepartmentPo>();

                if (entity.Id == 0)
                {
                    #region 新增部门信息
                    if (_departmentRepository.Any<DepartmentPo>(e => e.IsValid == 1 && e.Name == request.Entity.Name))
                    {
                        throw new Exception("部门名称已存在");
                    }

                    EntityLogger.CreateEntity(entity);
                    entity.Id = _departmentRepository.InsertReturnIdentity(entity);
                    #endregion

                }
                else
                {
                    #region 更新部门信息
                    var entitytemp = _departmentRepository.FindSingle<DepartmentPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新部门信息不存在");
                    }

                    EntityLogger.UpdateEntity(entity);

                    _departmentRepository.UpdateIgnoreColumns(entity, e => new
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
                LogManager.LogicLogger.ErrorFormat("保存部门出错:{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存用户部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserDepartmentResponse SaveUserDepartment(SaveUserDepartmentRequest request)
        {
            var response = new SaveUserDepartmentResponse();
            try
            {
                var list = _userDepartmentJoinRepository.FindWhere<UserDepartmentJoinPo>(e => e.IsValid == 1 && e.UserId == request.UserId);


                if (list != null && list.Count > 0)
                {
                    var templist = list.Where(e => e.DepartmentId != request.DepartmentId);

                    foreach (var item in templist)
                    {
                        item.IsValid = 0;
                        EntityLogger.UpdateEntity(item);

                        _userDepartmentJoinRepository.UpdateColumns(item, e => new
                        {
                            e.IsValid,
                            e.UpdateUserId,
                            e.UpdateTime
                        });
                    }

                    if (request.DepartmentId == 0 || list.Any(e => e.DepartmentId == request.DepartmentId))
                    {
                        return response;
                    }
                }

                //新增用户部门关系
                var entity = new UserDepartmentJoinPo
                {
                    UserId = request.UserId,
                    DepartmentId = request.DepartmentId
                };

                EntityLogger.CreateEntity(entity);
                _userDepartmentJoinRepository.InsertReturnIdentity(entity);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("保存用户部门出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelDepartmentResponse DelDeparment(DelDepartamentRequest request)
        {
            var response = new DelDepartmentResponse();
            try
            {
                var list = _departmentRepository.UseStoredProcedure<DepartmentPo>("proc_SystemDepartmentById_select", new { Id = request.Id });

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var entity = item;

                        EntityLogger.UpdateEntity(entity);
                        entity.IsValid = 0;

                        _departmentRepository.UpdateColumns(entity, e => new
                        {
                            e.IsValid,
                            e.UpdateTime,
                            e.UpdateUserId
                        });
                    }
                }
                else
                {
                    throw new Exception("删除部门信息不存在");
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();
                LogManager.LogicLogger.ErrorFormat("删除部门出错:{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}