using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Api.Models.DtoSmsLog
{
    public class UpdateSmsLog : BaseModel
    {
        public int ID { get; set; }
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
        public TimeSpan? Time { get; set; }
    }
}
