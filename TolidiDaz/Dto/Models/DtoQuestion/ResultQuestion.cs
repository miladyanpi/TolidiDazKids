using Dto.Models.Base;
using Dto.Models.DtoGroupQuestion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoQuestion
{
    public class ResultQuestion : BaseModel
    {
        public int ID { get; set; }
        public int? GroupQuestionID { get; set; }
        [DisplayName("عنوان")]
        
        public string? Title { get; set; }
        [DisplayName("تیتر")]
        public string? SubTitle { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

        public ResultGroupQuestion? ResultGroupQuestion { get; set; }



    }
}
