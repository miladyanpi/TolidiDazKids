using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoTicket
{
    public class UpdateTicket : BaseModel
    {
        public int ID { get; set; }
        public int? DepartmentID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("اولویت بلیط")]
        [Required(ErrorMessage = "| الزامی است")]
        public TicketPriority? TicketPriority { get; set; }
        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Description { get; set; }
        [DisplayName("فایل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? JsonTicketFile { get; set; }

    }
}
