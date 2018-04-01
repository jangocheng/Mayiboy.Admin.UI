using System;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.DataAccess.Repository;
using Mayiboy.Model.Po;

namespace Mayiboy.Logic.Impl
{
    /// <summary>
    /// 用户信息服务
    /// </summary>
    public class UserInfoService : BaseService, IUserInfoService
    {
        private readonly IUserInfoRepository _userInfoRepository;

        public UserInfoService()
        {
            _userInfoRepository = new UserInfoRepository();
        }


        public InsertResponse Insert(InsertRequest request)
        {
            var response = new InsertResponse();

            var entity = new UserInfoPo()
            {
                LoginName = "Simon",
                Password = "123456",
                Name = "蔡尚猛",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsValid = 1
            };

            entity = _userInfoRepository.InsertReturnEntity(entity);

            return response;
        }
    }
}