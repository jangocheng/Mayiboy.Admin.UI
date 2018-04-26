using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Areas.SystemManage.Models;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        // GET: SystemManage/UserInfo
        [LoginAuth]
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

        //删除用户
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