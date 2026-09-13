using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoAdvertisementSingle
{
    public class AddAdvertisementSingle:BaseModel
    {
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("نام سایت")]
        public string? SiteName { get; set; }
        [DisplayName("لینک")]
        public string? SiteUrl { get; set; }="#";
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
