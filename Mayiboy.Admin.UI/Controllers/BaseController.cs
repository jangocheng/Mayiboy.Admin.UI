﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;
using Mayiboy.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
    [LoginAuth]
    public class BaseController : Controller
    {
        private readonly IUserInfoService _iuserinfoservice;
        private readonly ISystemNavbarService _systemNavbarService;
        private readonly ISystemAppSettingsService _systemAppSettingsService;

        public BaseController()
        {
            _iuserinfoservice = ServiceLocater.GetService<IUserInfoService>();
            _systemNavbarService = ServiceLocater.GetService<ISystemNavbarService>();
            _systemAppSettingsService = ServiceLocater.GetService<ISystemAppSettingsService>();
        }

        #region 错误返回
        /// <summary>
        /// 错误返回
        /// </summary>
        /// <param name="status">错误状态码</param>
        /// <param name="msg">提示消息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToJsonErrorResult(long status = 1, string msg = "有错误", string buginfo = "")
        {
            return Json(new { status = status, msg = msg, buginfo = buginfo }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 成功返回
        /// <summary>
        ///成功返回 
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <returns></returns>
        public JsonResult ToJsonResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 异常返回
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="buginfo">错误提示(用于开发错误提示)</param>
        /// <returns></returns>
        public JsonResult ToJsonFatalResult(string msg, string buginfo = "")
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

        #region 获取系统配置
        /// <summary>
        /// 获取系统配置(Run time 1分钟 redis 1天)
        /// </summary>
        /// <param name="key">配置key</param>
        /// <returns></returns>
        public string GetSystemAppSetting(string key)
        {
            if (key.IsNullOrEmpty()) return null;

            var cachekey = key.AddCachePrefix("systemappsetting");

            var value = CacheManager.Get<string>(cachekey, 5);

            if (value.IsNullOrEmpty())
            {
                var response = _systemAppSettingsService.GetSysAppSetting(new GetSysAppSettingRequest
                {
                    Key = key
                });

                if (response.IsSuccess && response.KeyValue.IsNotNullOrEmpty())
                {
                    value = response.KeyValue;

                    CacheManager.RedisDefault.Set<string>(cachekey, value, PublicConst.Time.Day1);
                }
            }

            return value;
        }
        #endregion
    }
}