using System;

namespace Mayiboy.Contract
{
    [Serializable]
    public class BaseResponse
    {
        public BaseResponse()
        {
            IsSuccess = true;
        }
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回Code
        /// </summary>
        public string MessageCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string MessageText { get; set; }
    }
}