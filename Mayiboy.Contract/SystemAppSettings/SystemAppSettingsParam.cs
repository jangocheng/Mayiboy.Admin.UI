using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
    public class GetSysAppSettingRequest : Request
    {
        public string Key { get; set; }
    }

    public class GetSysAppSettingResponse : Response
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string KeyValue { get; set; }
    }

    public class DelSysAppSettingRequest : Request
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
    }

    public class DelSysAppSettingResponse : Response
    {

    }



    public class SaveSysAppSettingReqeust : Request
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        public SystemAppSettingsDto Entity { get; set; }
    }

    public class SaveSysAppSettingResponse : Response
    {

    }

    public class QuerySysAppSettingRequest : PageRequest
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

    public class QuerySysAppSettingResponse : PageResponse
    {
        /// <summary>
        /// 系统配置列表
        /// </summary>
        public List<SystemAppSettingsDto> EntityList { get; set; }
    }
}