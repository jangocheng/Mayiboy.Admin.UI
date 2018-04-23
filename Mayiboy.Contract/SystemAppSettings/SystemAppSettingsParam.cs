namespace Mayiboy.Contract
{
    public class GetSysAppSettingRequest : BaseRequest
    {
        public string Key { get; set; }
    }

    public class GetSysAppSettingResponse : BaseResponse
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string KeyValue { get; set; }
    }

    public class DelSysAppSettingRequest : BaseRequest
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelSysAppSettingResponse : BaseResponse
    {

    }



    public class SaveSysAppSettingReqeust : BaseRequest
    {
        
    }

    public class SaveSysAppSettingResponse : BaseResponse
    {
        
    }

    public class QuerySysAppSettingRequest : BasePageRequest
    {
        
    }

    public class QuerySysAppSettingResponse : BasePageResponse
    {
        
    }
}