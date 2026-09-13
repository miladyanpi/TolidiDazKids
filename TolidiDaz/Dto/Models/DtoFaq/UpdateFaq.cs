using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoFaq
{
    public class UpdateFaq: BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام کامل")]
        public string? FullName { get; set; }
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("موضوع")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Subject { get; set; }
        [DisplayName("پیام شما")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Message { get; set; }
        [DisplayName("آی پی")]
        public string? IpAddress { get; set; }
        [DisplayName("وضعیت پیام")]
        public bool IsRead { get; set; }
    }
}
