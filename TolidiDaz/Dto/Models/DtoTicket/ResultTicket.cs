using Dto.Models.Base;
using Dto.Models.DtoDepartment;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoTicket
{
    public class ResultTicket : BaseModel
    {
        public int ID { get; set; }
        public int? DepartmentID { get; set; }
        [DisplayName("عنوان")]
        public string? Title { get; set; }
        [DisplayName("اولویت بلیط")]
        public string? TicketPriority { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("فایل")]
        public string? JsonTicketFile { get; set; }

        public ResultDepartment? ResultDepartment { get; set; }



    }
}
