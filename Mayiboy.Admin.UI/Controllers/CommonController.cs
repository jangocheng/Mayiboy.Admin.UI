using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.Admin.UI.Controllers
{
    public class CommonController : BaseController
    {
        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginStatus()
        {
            var status = (LoginAccount.UserInfo == null ? -1 : 0);

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }
    }
}