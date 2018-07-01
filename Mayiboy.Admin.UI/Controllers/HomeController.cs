using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ISystemNavbarService _systemNavbarService;
		private readonly ISystemMenuService _systemMenuService;

		public HomeController(ISystemNavbarService systemNavbarService, ISystemMenuService systemMenuService)
		{
			_systemNavbarService = systemNavbarService;
			_systemMenuService = systemMenuService;
		}

		//
		public ActionResult Index()
		{
			ViewBag.LogAccount = LoginAccount.UserInfo;

			return View();
		}

		//获取系统导航栏
		public ActionResult SystemNavbar()
		{
			//获取系统栏目
			var list = new List<SystemNavbarModel>();

			var res = _systemNavbarService.QueryMavbarByUserId(new QueryMavbarByUserIdRequest
			{
				UserId = LoginAccount.UserInfo.Id
			});

			if (res.IsSuccess && res.EntityList != null)
			{
				list = res.EntityList.Select(e => e.As<SystemNavbarModel>()).ToList();
			}

			ViewBag.SysNavbarList = list;

			return View();
		}

		//获取系统菜单
		public ActionResult GetSystemMenu(string id)
		{
			try
			{
				var list = new List<SystemMenuModel>();

				var res = _systemMenuService.QueryMenuByUserId(new QueryMenuByUserIdRequest
				{
					NavbarId = int.Parse(id),
					UserId = LoginAccount.UserInfo.Id
				});

				if (res.IsSuccess && res.EntityList != null)
				{
					#region 初始化需要鉴权的地址
					res.EntityList.ForEach(e =>
								{
									if (!string.IsNullOrEmpty(e.UrlAddress) && e.AddressAuth != null & e.AddressAuth == 1)
									{
										e.UrlAddress = string.Concat(e.UrlAddress, (e.UrlAddress.Contains("?") ? "&" : "?"), PublicConst.UrlAuth, "=", LoginAccount.Identity);
									}
								}); 
					#endregion

					list = ToTree(res.EntityList);
				}

				return ToJsonResult(new { status = 0, data = list });
			}
			catch (Exception ex)
			{
				return ToJsonFatalResult("系统出错", ex.ToString());
			}
		}


		private List<SystemMenuModel> ToTree(List<SystemMenuDto> list, int id = 0)
		{
			var treelist = new List<SystemMenuModel>();

			if (list == null || list.Count == 0) return null;

			if (list.Any(e => e.Pid == id))
			{
				treelist.AddRange(list.Where(e => e.Pid == id).Select(e => e.As<SystemMenuModel>()).OrderBy(e => e.Sort));
			}
			else
			{
				return null;
			}

			foreach (var item in treelist)
			{
				if (list.Any(o => o.Pid == item.Id))
				{
					item.ChildNodes = ToTree(list, item.Id);
				}
			}

			return treelist;
		}

		//首页
		public ActionResult Page()
		{
			return View();
		}

		//介绍
		public ActionResult About()
		{
			return View();
		}
	}
}