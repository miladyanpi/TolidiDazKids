using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoAbout
{
    public class UpdateAbout : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("متن خلاصه")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? ShortDescription { get; set; }
        [DisplayName("متن کامل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? LongDescription { get; set; }
        [DisplayName("تصاویر")]
        public string? JsonPicture { get; set; }
        public List<ResultUploadFile>? ResultUploadFiles { get; set; }

    }
}
