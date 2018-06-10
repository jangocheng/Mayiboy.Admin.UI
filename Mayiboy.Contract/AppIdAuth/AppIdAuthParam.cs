using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
    public class SaveAppIdAuthRequest : Request
    {
        public AppIdAuthDto Entity { get; set; }
    }

    public class SaveAppIdAuthResponse : Response
    {
        public int Id { get; set; }
    }

    public class QueryAppIdAuthRequest : PageRequest
    {
        public string ServiceAppId { get; set; }

        public string AuthToken { get; set; }
    }

    public class QueryAppIdAuthResponse : PageResponse
    {
        public List<AppIdAuthDto> EntityList { get; set; }
    }

    public class DelAppIdAuthRequest : Request
    {
        public int Id { get; set; }
    }

    public class DelAppIdAuthResponse : Response
    {

    }

    public class UpdateStatusRequest : Request
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 启用状态（0:未启用；1：已启用）
        /// </summary>
        public int Status { get; set; }
    }

    public class UpdateStatusResponse : Response
    {

    }

    public class QueryByAppIdRequest : Request
    {
        /// <summary>
        /// 应用授权标识
        /// </summary>
        public string ServiceAppId { get; set; }
    }

    public class QueryByAppIdResponse : Response
    {
        public AppIdAuthDto Entity { get; set; }
    }


    public class SaveSecretKeyRequest : Request
    {
        public AppIdAuthDto Entity { get; set; }
    }

    public class SaveSecretKeyResponse : Response
    {

    }

    public class GetAppIdAuthRequest : Request
    {
        public int Id { get; set; }
    }

    public class GetAppIdAuthResponse : Response
    {
        public AppIdAuthDto Entity { get; set; }
    }


}