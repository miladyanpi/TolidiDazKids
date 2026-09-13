using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoReminderSrv
{

    public class AddReminder : BaseModel
    {
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("تاریخ ارسال")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Date { get; set; }
        [DisplayName("زمان ارسال")]
        [Required(ErrorMessage = "| الزامی است")]
        public TimeSpan Time{ get; set; }
        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Description { get; set; }
        [DisplayName("ارسال پیامک")]
        public bool SendSms { get; set; }

    }
}
