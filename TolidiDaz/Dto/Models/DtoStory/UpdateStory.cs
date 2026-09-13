using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoStory
{
    public class UpdateStory:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("لینک")]
        public string? Url { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("ویدئو")]
        public string? JsonVideo { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
