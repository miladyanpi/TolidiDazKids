using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoAdvertisement
{
    public class ResultAdvertisement : BaseModel
    {
        public int ID { get; set; }

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("نام سایت")]
        public string? SiteName { get; set; }
        [DisplayName("لینک")]
        public string? SiteUrl { get; set; } = "#";
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
