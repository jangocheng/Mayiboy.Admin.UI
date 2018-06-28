using System;
using System.Text.RegularExpressions;
using AutoMapper;
using Mayiboy.Contract;
using Mayiboy.Model;
using Mayiboy.Model.Po;

namespace Mayiboy.Logic.Mapper
{
    /// <summary>
    /// 映射配置关系
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// 映射配置关系
        /// </summary>
        public MapperProfile()
        {
            #region 默认映射关系

            //DateTime to String
            this.CreateMap<DateTime?, string>().ConvertUsing(e =>
            {
                if (!e.HasValue) return string.Empty;

                return e.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            });


            //String To DateTime

            this.CreateMap<string, DateTime?>().ConstructUsing(e =>
            {
                if (string.IsNullOrEmpty(e)) return null;

                if (e.Length == 8)
                {
                    if (Regex.IsMatch(e, "^[0-9]{4}[0-9]{2}[0-9]{2}$", RegexOptions.IgnoreCase))
                    {
                        return new DateTime(int.Parse(e.Substring(0, 4)), int.Parse(e.Substring(4, 2)), int.Parse(e.Substring(6, 2)));
                    }
                }

                return DateTime.Parse(e);
            });
            #endregion

            //用户
            this.CreateMap<UserInfoPo, UserInfoDto>();
            this.CreateMap<UserInfoDto, UserInfoPo>();
            this.CreateMap<UserInfoDto, AccountModel>();
            this.CreateMap<AccountModel, UserInfoDto>();

            //用户角色
            this.CreateMap<UserRoleJoinPo, UserRoleJoinDto>();
            this.CreateMap<UserRoleJoinDto, UserRoleJoinPo>();

            //系统栏目
            this.CreateMap<SystemNavbarPo, SystemNavbarDto>();
            this.CreateMap<SystemNavbarDto, SystemNavbarPo>();
            this.CreateMap<SystemNavbarDto, SystemNavbarModel>();
            this.CreateMap<SystemNavbarModel, SystemNavbarDto>();

            //系统菜单
            this.CreateMap<SystemMenuPo, SystemMenuDto>();
            this.CreateMap<SystemMenuDto, SystemMenuPo>();
            this.CreateMap<SystemMenuDto, SystemMenuModel>();
            this.CreateMap<SystemMenuModel, SystemMenuDto>();

            //部门
            this.CreateMap<DepartmentPo, DepartmentDto>();
            this.CreateMap<DepartmentDto, DepartmentPo>();

            //系统配置
            this.CreateMap<SystemAppSettingsPo, SystemAppSettingsDto>();
            this.CreateMap<SystemAppSettingsDto, SystemAppSettingsPo>();

            //权限配置
            this.CreateMap<PermissionsPo, PermissionsDto>();
            this.CreateMap<PermissionsDto, PermissionsPo>();

            //系统日志
            this.CreateMap<SystemOperationLogPo, SystemOperationLogDto>();
            this.CreateMap<SystemOperationLogDto, SystemOperationLogPo>();

            //应用授权
            this.CreateMap<AppIdAuthPo, AppIdAuthDto>();
            this.CreateMap<AppIdAuthDto, AppIdAuthPo>();

			//用户授权AppId
	        this.CreateMap<UserAppIdAuthPo, UserAppIdAuthDto>();
	        this.CreateMap<UserAppIdAuthDto, UserAppIdAuthPo>();
        }
    }
}