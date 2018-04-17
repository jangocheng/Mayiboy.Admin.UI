namespace Mayiboy.Contract
{

    /// <summary>
    /// 查询所有导航栏下的菜单参数
    /// </summary>
    public class QueryAllMenuRequest : BaseRequest
    {
        /// <summary>
        /// 导航栏Id
        /// </summary>
        public int NavbarId { get; set; }
    }

    /// <summary>
    /// 查询所有导航栏下的菜单参数
    /// </summary>
    public class QueryAllMenuResponse : BaseResponse
    {

    }

    /// <summary>
    /// 根据用户名Id查询导航栏下的所有菜单参数
    /// </summary>
    public class QueryMenuByUserIdRequest : BaseRequest
    {
        /// <summary>
        /// 导航栏Id
        /// </summary>
        public int NavbarId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询导航栏下的所有菜单响应
    /// </summary>
    public class QueryMenuByUserIdResponse : BaseResponse
    {

    }
}