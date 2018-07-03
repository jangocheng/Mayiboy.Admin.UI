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
	public class AppProjectController : BaseController
	{
		private readonly IAppProjectService _appProjectService;

		public AppProjectController(IAppProjectService appProjectService)
		{
			_appProjectService = appProjectService;
		}

		public ActionResult Index()
		{
			return View();
		}

		//查询应用项目
		public ActionResult Query(string name, string appid, int page = 1, int limit = 20)
		{
			try
			{
				var response = _appProjectService.QueryAppProject(new QueryAppProjectRequest
				{
					ProjectName = name,
					ApplicationId = appid,
					PageIndex = page,
					PageSize = limit

				});

				if (!response.IsSuccess)
				{
					return ToJsonErrorResult(1, "查询出错", response.MessageText);
				}
				return Json(new { code = 0, data = response.EntityList, count = response.TotalCount }, JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.ErrorFormat("查询应用项目出错：{0}", new { err = ex.ToString() }.ToJson());
				return Json(new { status = -1, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
			}
		}

		//保存应用项目
		public ActionResult Save(string name, string appid, string remark)
		{
			try
			{
				if (name.IsNullOrEmpty() || appid.IsNullOrEmpty())
				{
					return ToJsonErrorResult(1, "参数不能为空");
				}

				var response = _appProjectService.SaveAppProject(new SaveAppProjectRequest
				{
					Entity = new AppProjectDto
					{
						ProjectName = name,
						ApplicationId = appid,
						Remark = remark
					}
				});

				if (!response.IsSuccess)
				{
					return ToJsonErrorResult(1, response.MessageText);
				}

				return ToJsonResult(new { status = 0 });
			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.ErrorFormat("保存应用项目出错：{0}", new { err = ex.ToString() }.ToJson());
				return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
			}
		}

		//删除项目应用
		public ActionResult Del(string id)
		{
			try
			{
				var response = _appProjectService.DelAppProject(new DelAppProjectRequest
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
				LogManager.DefaultLogger.ErrorFormat("删除应用项目出错：{0}", new { err = ex.ToString() }.ToJson());
				return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}