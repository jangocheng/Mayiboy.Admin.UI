using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Areas.SystemManage.Models;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IUserRoleService _userRoleService;

        public UserInfoController(IUserInfoService userInfoService, IUserRoleService userRoleService)
        {
            _userInfoService = userInfoService;
            _userRoleService = userRoleService;
        }

        // GET: SystemManage/UserInfo
        public ActionResult Index()
        {
            return View();
        }

        //查询用户
        public ActionResult Query(string account, string sex, string departmentid, string userroleid, int page = 1, int limit = 20)
        {
            try
            {
                var response = _userInfoService.QueryUserInfo(new QueryUserInfoRequest
                {
                    Account = account,
                    Sex = int.Parse(sex.IsNullOrEmpty() ? "-1" : sex),
                    PageIndex = page,
                    PageSize = limit
                });

                if (!response.IsSuccess)
                {
                    return Json(new { status = 1, msg = "查询出错" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { code = 0, data = response.EntityList, count = response.TotalCount }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询用户信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存用户信息
        [ActionAuth]
        [OperLog("保存系统用户信息")]
        public ActionResult Save(UserInfoModel model)
        {
            try
            {
                var entity = new UserInfoDto
                {
                    Id = model.Id,
                    LoginName = model.LoginName,
                    Name = model.Name,
                    Email = model.Email,
                    Password = GetSystemAppSetting("SystemDefaultPassword").GetMd5(),
                    Sex = model.Sex,
                    Mobile = model.Mobile,
                    Remark = model.Remark,
                    HeadimgUrl = GetSystemAppSetting("SystemDefaultHeadimg")
                };


                var response = _userInfoService.SaveUserInfo(new SaveUserInfoRequest { UserInfoEntity = entity });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存用户信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }

        }

        //重置用户密码
        [ActionAuth]
        [OperLog("重置用户密码")]
        public ActionResult ResetPassword(string userid)
        {
            try
            {
                var response = _userInfoService.ResetPassword(new ResetPasswordRequest
                {
                    UserId = int.Parse(userid),
                    NewPassword = GetSystemAppSetting("SystemDefaultPassword")
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(2, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("重置用户密码出错：{0}", new { userid, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("重置密码出错！");
            }
        }

        //查询所有用户角色
        public ActionResult QueryAllUserRole(string userid)
        {
            try
            {
                var userrolelist = new List<UserRoleDto>();
                var userrolejoinlist = new List<UserRoleJoinDto>();

                #region 角色列表
                var response = _userRoleService.QueryUserRole(new QueryUserRoleRequest()
                {
                    Name = "",
                    PageIndex = 1,
                    PageSize = int.MaxValue
                });

                if (response.IsSuccess)
                {
                    userrolelist = response.List;
                }
                #endregion

                #region 用户角色
                var userrolejoinres = _userRoleService.QueryUserRoleJoin(new QueryUserRoleJoinRequest
                {
                    UserId = int.Parse(userid)
                });

                if (userrolejoinres.IsSuccess)
                {
                    userrolejoinlist = userrolejoinres.EntityList;
                }
                #endregion

                var list = userrolelist.Select(e => new
                {
                    LAY_CHECKED = userrolejoinlist.Any(o => o.RoleId == e.Id),
                    Id = e.Id,
                    e.Name,
                    e.Remark
                });

                //查询用户角色

                return ToJsonResult(new { code = 0, data = list });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询用户角色出错：{0}", new { userid, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("查询用户角色出错");
            }
        }

        //保存用户角色
        [ActionAuth]
        public ActionResult SaveUserRole(string userid, List<int> roleid)
        {
            try
            {
                var response = _userRoleService.SaveUserRoleJoin(new SaveUserRoleJoinRequest
                {
                    UserId = int.Parse(userid),
                    RoleIdList = roleid
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                return ToJsonFatalResult(ex.Message);
            }
        }

        //删除用户
        [ActionAuth]
        [OperLog("删除系统用户信息")]
        public ActionResult Del(string id)
        {
            try
            {
                var response = _userInfoService.Del(new DelUserInfoRequest()
                {
                    Id = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除系统栏目出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}