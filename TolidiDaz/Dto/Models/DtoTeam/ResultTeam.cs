using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoTeam
{
    public class ResultTeam:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام ")]
        public string? Name { get; set; }
        [DisplayName("سمت/جایگاه/تخصص ")]
        public string? Title { get; set; }
        [DisplayName("توضیحات ")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPictures { get; set; }
        [DisplayName("نمایش در درباره ما")]
        public bool ShowInAbout { get; set; }
        public List<ResultUploadFile>? ResultUploadFiles { get; set; }

    }
}
