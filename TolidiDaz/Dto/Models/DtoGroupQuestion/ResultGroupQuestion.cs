using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoGroupQuestion
{
    public class ResultGroupQuestion : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        public string? Title { get; set; }

    }
}
