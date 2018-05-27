using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{

    public class GetRequest : Request
    {
        public int Id { get; set; }
    }

    public class GetResponse : Response
    {
        public SystemNavbarDto Entity { get; set; }
    }

    /// <summary>
    /// 查询所系统所有导航栏参数
    /// </summary>
    public class QueryAllNavbarRequest : Request
    {

    }

    /// <summary>
    /// 查询所系统所有导航栏响应
    /// </summary>
    public class QueryAllNavbarResponse : Response
    {
        /// <summary>
        /// 系统导航栏列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询系统导航栏列表参数
    /// </summary>
    public class QueryNavbarRequest : Request
    {
        public int UserId { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询系统导航栏列表响应
    /// </summary>
    public class QueryNavbarResponse : Response
    {
        /// <summary>
        /// 系统导航栏列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }



    public class QueryRequest : PageRequest
    {
        /// <summary>
        /// 栏目名
        /// </summary>
        public string Name { get; set; }
    }

    public class QueryResponse : PageResponse
    {
        /// <summary>
        /// 系统栏目列表
        /// </summary>
        public List<SystemNavbarDto> SystemNavbarList { get; set; }
    }



    public class SaveRequest : Request
    {
        /// <summary>
        /// 系统栏目实例
        /// </summary>
        public SystemNavbarDto Entity { get; set; }
    }

    public class SaveResponse : Response
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelReqeust : Request
    {
        /// <summary>
        /// 栏目Id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelResponse : Response
    {

    }


    public class QueryMavbarByUserIdRequest : Request
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }

    public class QueryMavbarByUserIdResponse : Response
    {
        /// <summary>
        /// 系统栏目
        /// </summary>
        public List<SystemNavbarDto> EntityList { get; set; }
    }

}