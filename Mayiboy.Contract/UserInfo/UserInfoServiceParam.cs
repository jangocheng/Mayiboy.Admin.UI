using System.Collections.Generic;
using Mayiboy.Model.Dto;

namespace Mayiboy.Contract
{

    /// <summary>
    /// 插入用户信息参数
    /// </summary>
    public class SaveUserInfoRequest : BaseRequest
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto UserInfoEntity { get; set; }
    }

    /// <summary>
    /// 插入用户信息响应参数
    /// </summary>
    public class SaveUserInfoResponse : BaseResponse
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto UserInfoEntity { get; set; }
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
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto UserInfoEntity { get; set; }
    }

    /// <summary>
    /// 查询用户信息参数
    /// </summary>
    public class QueryUserInfoRequest : BasePageRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别（-1：全部；0:女；1：男）
        /// </summary>
        public int? Sex { get; set; }
    }

    /// <summary>
    /// 查询用户信息响应
    /// </summary>
    public class QueryUserInfoResponse : BasePageResponse
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserInfoDto> EntityList { get; set; }
    }


    public class DelUserInfoRequest : BaseRequest
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelUserInfoResponse : BaseResponse
    {
        /// <summary>
        /// 删除的用户信息
        /// </summary>
        public UserInfoDto Entity { get; set; }
    }
}