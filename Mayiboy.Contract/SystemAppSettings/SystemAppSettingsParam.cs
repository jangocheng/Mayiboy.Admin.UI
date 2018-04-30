using System.Collections.Generic;
using Mayiboy.Model.Dto;

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
        /// <summary>
        /// 系统配置
        /// </summary>
        public SystemAppSettingsDto Entity { get; set; }
    }

    public class SaveSysAppSettingResponse : BaseResponse
    {

    }

    public class QuerySysAppSettingRequest : BasePageRequest
    {
        /// <summary>
        /// 字典名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KeyValue { get; set; }

        /// <summary>
        /// 字典描述
        /// </summary>
        public string Remark { get; set; }
    }

    public class QuerySysAppSettingResponse : BasePageResponse
    {
        /// <summary>
        /// 系统配置列表
        /// </summary>
        public List<SystemAppSettingsDto> EntityList { get; set; }
    }
}