using System;
using SqlSugar;

namespace Mayiboy.Model.Po
{
    [SugarTable("AppIdAuth")]
    public class AppIdAuthPo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 授权Token
        /// </summary>
        public string AuthToken { get; set; }

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
        /// 创建用户Id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}