using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Api.Models.DtoReminderEvent
{
    public class UpdateReminderEvent : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("زمان ارسال")]
        [Required(ErrorMessage = "| الزامی است")]
        public TimeSpan Time { get; set; }
        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Description { get; set; }
        [DisplayName("ارسال پیامک")]
        public bool SendSms { get; set; }
        [DisplayName("نوع پیام")]
        public ReminderType ReminderType { get; set; }

    }
}
