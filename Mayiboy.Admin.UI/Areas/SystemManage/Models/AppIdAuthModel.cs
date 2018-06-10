using System.ComponentModel.DataAnnotations;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Models
{
    public class AppIdAuthModel : BaseModel
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [Required(ErrorMessage = "应用标识不能为空")]
        public string Appid { get; set; }

        /// <summary>
        /// 授权Token
        /// </summary>
        [Required(ErrorMessage = "授权Token不能为空")]
        public string Authtoken { get; set; }

        /// <summary>
        /// 接口数据加密类型（0：不加密；1：对称加密（DES）；2：对称加密（AES）；3：非对称加密
        /// </summary>
        public int EncryptionType { get; set; }

        /// <summary>
        /// 对称加密-秘钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 非对称加密-私钥
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// 非对称加密-公钥
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// 是否启用状态（0：未启用；1：已启用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}