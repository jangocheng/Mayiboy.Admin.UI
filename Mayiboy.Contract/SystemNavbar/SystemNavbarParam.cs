using System.Collections.Generic;
using Mayiboy.Model.Dto;

namespace Mayiboy.Contract
{

    public class GetRequest : BaseRequest
    {
        public int Id { get; set; }
    }

    public class GetResponse : BaseResponse
    {
        public SystemNavbarDto Entity { get; set; }
    }

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



    public class QueryRequest : BasePageRequest
    {
        /// <summary>
        /// 栏目名
        /// </summary>
        public string Name { get; set; }
    }

    public class QueryResponse : BasePageResponse
    {
        /// <summary>
        /// 系统栏目列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }



    public class SaveRequest : BaseRequest
    {
        /// <summary>
        /// 系统栏目实例
        /// </summary>
        public SystemNavbarDto Entity { get; set; }
    }

    public class SaveResponse : BaseResponse
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelReqeust : BaseRequest
    {
        /// <summary>
        /// 栏目Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelResponse : BaseResponse
    {

    }


    public class QueryMavbarByUserIdRequest : BaseRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }

    public class QueryMavbarByUserIdResponse : BaseResponse
    {
        /// <summary>
        /// 系统栏目
        /// </summary>
        public List<SystemNavbarDto> EntityList { get; set; }
    }

}