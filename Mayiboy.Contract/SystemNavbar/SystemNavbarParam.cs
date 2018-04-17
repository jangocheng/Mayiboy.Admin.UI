using System.Collections.Generic;
using Mayiboy.Model.Dto;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 查询所系统所有导航栏参数
    /// </summary>
    public class QueryAllNavbarRequest : BaseRequest
    {

    }

    /// <summary>
    /// 查询所系统所有导航栏响应
    /// </summary>
    public class QueryAllNavbarResponse : BaseResponse
    {
        /// <summary>
        /// 系统导航栏列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询系统导航栏列表参数
    /// </summary>
    public class QueryNavbarRequest : BaseRequest
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询系统导航栏列表响应
    /// </summary>
    public class QueryNavbarResponse : BaseResponse
    {
        /// <summary>
        /// 系统导航栏列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }
}