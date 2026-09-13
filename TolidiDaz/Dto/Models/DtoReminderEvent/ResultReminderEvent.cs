using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Api.Models.DtoReminderEvent
{
    public class ResultReminderEvent : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("زمان ارسال")]
        [Required(ErrorMessage = "| الزامی است")]
        public TimeSpan Time { get; set; }

        [DisplayName("متن پیام")]
        public string? Description { get; set; }

        [DisplayName("ارسال پیامک")]
        public bool SendSms { get; set; }
        [DisplayName("نوع پیام")]
        public string? ReminderType { get; set; }

    }
}
