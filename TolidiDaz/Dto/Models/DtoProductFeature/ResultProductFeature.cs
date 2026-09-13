using Dto.Models.Base;
using Dto.Models.DtoCategory;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProductFeature
{
    public class ResultProductFeature : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("دسته بندی")]
        public int? CategoryID { get; set; }
        [DisplayName("عنوان ویژگی")]
        public string? Title { get; set; }
        public ResultCategory? ResultCategory { get; set; }

    }
}
