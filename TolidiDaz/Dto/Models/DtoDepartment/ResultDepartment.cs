using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoDepartment
{
    public class ResultDepartment : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        public string? Title { get; set; }

    }
}
