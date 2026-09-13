using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoSmsOtpCode
{
    public class ResultSmsOtpCode : BaseModel
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
        public int? DateExpired { get; set; }
        [DisplayName("زمان انقضا")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? TimeExpired { get; set; }
    }
}
