using System.ComponentModel.DataAnnotations;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class LoginUserInfoModel : BaseModel
    {
        private string _username;

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value.IsNotNullOrEmpty() ? RsaCryption.RsaDecrypt(PublicConst.XmlPrivateKey, value) : value;
            }
        }

        private string _passWord;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填")]
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value.IsNotNullOrEmpty() ? RsaCryption.RsaDecrypt(PublicConst.XmlPrivateKey, value) : value;
            }
        }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码必填")]
        public string Code { get; set; }
    }
}