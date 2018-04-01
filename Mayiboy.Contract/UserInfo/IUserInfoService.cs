namespace Mayiboy.Contract
{
    public interface IUserInfoService : IBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        InsertResponse Insert(InsertRequest request);
    }
}