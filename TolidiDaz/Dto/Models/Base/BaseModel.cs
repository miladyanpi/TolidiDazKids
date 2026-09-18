

using Dto.Models.DtoLable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.Base
{
    public class BaseModel
    {
        public BaseModel()
        {
            IdentityCode = Guid.NewGuid();
            Visible = true;
        }
        public Guid? IdentityCode { get; set; }
        [DisplayName("تاریخ ثبت")]
        public string? RegisterDate { get; set; }
        [DisplayName("زمان ثبت")]
        public TimeSpan RegisterTime { get; set; }
        [DisplayName("تاریخ ویرایش")]
        public string? EditDate { get; set; }
        [DisplayName("زمان ویرایش")]
        public TimeSpan EditTime { get; set; }
        [DisplayName("اسلاگ")]
        public string? Slug { get; set; }
        [DisplayName("برچسب ها")]
        public string? JsonLableTexts { get; set; }

        public List<ResultJsonLable>? ResultJsonLables { get; set; }
        [DisplayName("فعال/غیرفعال")]
        public bool Visible { get; set; }

    }
}
