using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoFaq
{
    public class ResultFaq : BaseModel
    {
        public int ID { get; set; }

        [DisplayName("نام کامل")]
        public string? FullName { get; set; }

        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("موضوع")]
        public string? Subject { get; set; }
        [DisplayName("پیام شما")]
        public string? Message { get; set; }
        [DisplayName("آی پی")]
        public string? IpAddress { get; set; }
        [DisplayName("وضعیت پیام")]
        public string? IsRead { get; set; }

    }
}
