
using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoSmsLog
{

    public class AddSmsLog : BaseModel
    {
        public int? CustomerID { get; set; }
        [DisplayName("عنوان پیام")]
        public string? Title { get; set; }
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Message { get; set; }
        [DisplayName("وضعیت پیام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? StatusText { get; set; }
        public int? SendDate { get; set; }
        public TimeSpan Time { get; set; }

    }
}
