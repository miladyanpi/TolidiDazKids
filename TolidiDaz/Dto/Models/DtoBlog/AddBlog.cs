using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoBlog
{
    public class AddBlog:BaseModel
    {
        [DisplayName("دسته بندی")]
        public int? GroupBlogID { get; set; }
        [DisplayName("نویسنده(از تیم تخصصی)")]
        public int? TeamID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("مدت زمان مطالعه")]
        public string? StudyDuration { get; set; }
        [DisplayName("تعداد بازدید")]
        public int NumberOfVisits { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }


    }
}
