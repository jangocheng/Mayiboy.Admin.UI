using System.ComponentModel;
using System.Linq;
using Framework.Mayiboy.Utility;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 验证数据异常
    /// </summary>
    public class BaseModel: IDataErrorInfo
    {
        private bool Valid { get; set; }

        public string Error
        {
            get
            {
                return "";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (Valid) return string.Empty;

                Valid = this.IsValid();

                if (!Valid)
                {
                    var list = this.ValidationResult();

                    if (list.Any())
                    {
                        var v = string.Join("\n", list.Select(e => e.ErrorMessage));

                        throw new ViewModelException(v);
                    }
                }

                return string.Empty;
            }
        }
    }
}