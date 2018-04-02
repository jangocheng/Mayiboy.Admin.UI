namespace Mayiboy.Contract
{

    /// <summary>
    /// 插入用户信息参数
    /// </summary>
    public class InsertRequest : BaseRequest
    {

    }

    /// <summary>
    /// 插入用户信息响应参数
    /// </summary>
    public class InsertResponse : BaseResponse
    {

    }



    /// <summary>
    /// 登录用户查询参数
    /// </summary>
    public class LoginQueryRequest : BaseRequest
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// 登录用户查询响应
    /// </summary>
    public class LoginQueryResponse : BaseResponse
    {
        public string Content { get; set; }
    }
}