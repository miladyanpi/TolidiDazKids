using Dto.Models.Base;
using Dto.Models.DtoGroupBlog;
using Dto.Models.DtoTeam;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoBlog
{
    public class ResultBlog : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("دسته بندی")]
        public int? GroupBlogID { get; set; }
        [DisplayName("عنوان")]
        public string? Title { get; set; }
        [DisplayName("مدت زمان مطالعه")]
        public string? StudyDuration { get; set; }
        [DisplayName("نویسنده(از تیم تخصصی)")]
        public int? TeamID { get; set; }
        [DisplayName("تعداد بازدید")]
        public int NumberOfVisits { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public string? Labeles { get; set; }
        [DisplayName("گروه مقاله")]
        public ResultGroupBlog? ResultGroupBlog { get; set; } = new();
        [DisplayName("نویسنده(در تیم تخصصی)")]
        public ResultTeam? ResultTeam { get; set; } = new();
    }
}
