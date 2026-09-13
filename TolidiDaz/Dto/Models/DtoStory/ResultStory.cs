using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoStory
{
    public class ResultStory:BaseModel
    {
        public int ID { get; set; }

        [DisplayName("عنوان")]
        public string? Title { get; set; }
        [DisplayName("لینک")]
        public string? Url { get; set; }
        [DisplayName("عکس")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? JsonPicture { get; set; }
        [DisplayName("ویدئو")]
        public string? JsonVideo { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
