using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysNavbarController : BaseController
    {
        private readonly ISystemNavbarService _systemNavbarService;

        public SysNavbarController(ISystemNavbarService systemNavbarService)
        {
            _systemNavbarService = systemNavbarService;
        }

        // GET: SystemManage/SysNavbar
        public ActionResult Index()
        {
            return View();
        }

        //查询栏目
        public ActionResult Query(string name, int page = 1, int limit = 20)
        {
            try
            {
                var response = _systemNavbarService.Query(new QueryRequest
                {
                    Name = name,
                    PageIndex = page,
                    PageSize = limit
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return Json(new { code = 0, data = response.SystemNavbarList, count = response.TotalCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询系统栏目出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存栏目
        public ActionResult Save(string name, string url, string remark, int sort = 0, int id = 0)
        {
            try
            {
                var request = new SaveRequest
                {
                    Entity = new Model.Dto.SystemNavbarDto
                    {
                        Id = id,
                        Name = name,
                        Url = url,
                        Sort = sort,
                        Remark = remark
                    }
                };

                var response = _systemNavbarService.Save(request);

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "保存出错！");
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存系统栏目出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //删除栏目
        public ActionResult Del(string id)
        {
            try
            {
                var response = _systemNavbarService.Del(new DelReqeust
                {
                    Id = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return Json(new { status = 1, msg = response.MessageText }, JsonRequestBehavior.AllowGet);
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