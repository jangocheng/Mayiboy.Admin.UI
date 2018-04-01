using System;

namespace Mayiboy.Contract
{
    [Serializable]
    public class BaseRequest
    {
        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId { get; set; }
    }
}