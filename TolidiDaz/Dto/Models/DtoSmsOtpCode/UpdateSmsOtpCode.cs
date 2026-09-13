using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoSmsOtpCode
{
    public class UpdateSmsOtpCode : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شماره موبایل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? PhoneNumber { get; set; }
        [DisplayName("کد یکبارمصرف")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Code { get; set; }
        [DisplayName("تاریخ انقضا")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? DateExpired { get; set; }
        [DisplayName("زمان انقضا")]
        [Required(ErrorMessage = "| الزامی است")]
        public TimeSpan TimeExpired { get; set; }
    }
}
