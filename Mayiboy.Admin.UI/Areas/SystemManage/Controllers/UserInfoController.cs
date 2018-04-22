using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class UserInfoController : BaseController
    {
        // GET: SystemManage/UserInfo
        public ActionResult Index()
        {
            return View();
        }

        //查询用户
        public ActionResult Query(string name, string departmentid,string userroleid, int page = 1, int limit = 20)
        {
            try
            {
                return ToJsonResult();
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询用户信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存用户信息
        public ActionResult Save()
        {
            return ToJsonResult();
        }

        //删除用户
        public ActionResult Del(string id)
        {
            try
            {
                return ToJsonResult();
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除系统栏目出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}