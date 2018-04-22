using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserInfoService _iuserinfoservice;
        private readonly ISystemNavbarService _systemNavbarService;

        public BaseController()
        {
            _iuserinfoservice = ServiceLocater.GetService<IUserInfoService>();
            _systemNavbarService = ServiceLocater.GetService<ISystemNavbarService>();
        }

        #region 错误返回
        /// <summary>
        /// 错误返回
        /// </summary>
        /// <param name="status">错误状态码</param>
        /// <param name="msg">提示消息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToErrorJsonResult(long status = 1, string msg = "有错误", string buginfo = "")
        {
            return Json(new { status = status, msg = msg, buginfo = buginfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 成功返回
        /// <summary>
        ///成功返回 
        /// </summary>
        /// <param name="obj">响应数据</param>
        /// <returns></returns>
        public JsonResult ToJsonResult(object obj = null)
        {
            return Json(new { status = 0, data = obj }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 异常返回
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToFatalJsonResult(string msg, string buginfo = "")
        {
            return Json(new { status = -1, msg = msg, buginfo = buginfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取全部栏目
        /// <summary>
        /// 获取全部栏目
        /// </summary>
        /// <returns></returns>
        public List<SystemNavbarModel> QueryAllSystemNavbar()
        {
            var list = new List<SystemNavbarModel>();

            try
            {
                var response = _systemNavbarService.QueryAll(new QueryAllNavbarRequest { });

                if (response.IsSuccess)
                {
                    list = response.SystemNavbarList.Select(e => e.As<SystemNavbarModel>()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("获取系统栏目出错：{0}", new { err = ex.ToString() }.ToJson());
            }

            return list;
        }
        #endregion
    }
}