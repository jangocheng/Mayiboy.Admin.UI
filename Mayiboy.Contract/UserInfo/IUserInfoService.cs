using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserInfoService : IBaseService, IDependency
    {
        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveUserInfoResponse SaveUserInfo(SaveUserInfoRequest request);

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryUserInfoResponse QueryUserInfo(QueryUserInfoRequest request);

		/// <summary>
		/// 根据用户名查询用户列表
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
	    QueryUserInfoByNameResponse QueryUserInfoByName(QueryUserInfoByNameRequest request);

		/// <summary>
		/// 根据用户Id查询用户信息
		/// </summary>
		/// <param name="request">查询参数</param>
		/// <returns></returns>
	    QueryUserInfoByIdResponse QueryUserInfoById(QueryUserInfoByIdRequest request);

		/// <summary>
		/// 删除用户信息
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		DelUserInfoResponse Del(DelUserInfoRequest request);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LoginQueryResponse LoginQuery(LoginQueryRequest request);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ResetPasswordResponse ResetPassword(ResetPasswordRequest request);

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
    }
}