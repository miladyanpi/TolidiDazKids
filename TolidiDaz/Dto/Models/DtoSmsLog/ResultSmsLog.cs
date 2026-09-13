using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoSmsLog
{
    public class ResultSmsLog : BaseModel
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
        public string? SendDate { get; set; }
        public string? Time { get; set; }
        public ResultCustomer? ResultCustomer { get; set; }
    }
}
