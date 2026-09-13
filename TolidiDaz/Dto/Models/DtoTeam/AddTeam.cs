using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoTeam
{
    public class AddTeam : BaseModel
    {
        [DisplayName("نام ")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Name { get; set; }
        [DisplayName("سمت/جایگاه/تخصص ")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }

        [DisplayName("توضیحات ")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPicture { get; set; }
        [DisplayName("نمایش در صفحه درباره ما")]
        public bool ShowInAbout { get; set; }

    }
}
