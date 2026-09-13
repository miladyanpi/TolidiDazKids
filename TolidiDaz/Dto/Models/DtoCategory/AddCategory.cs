
using Dto.Attributes;
using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoCategory
{
    public class AddCategory : BaseModel
    {
        [DisplayName("دسته بندی سطح 1")]
        public int? ParentID1 { get; set; }
        [DisplayName("دسته بندی سطح 2")]
        public int? ParentID2 { get; set; }
        [DisplayName("عنوان دسته بندی")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("اسلاگ")]
        public string? Slug { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPicture { get; set; }
        [DisplayName("ترتیب")]
        public int Order { get; set; }
        [DisplayName("تعداد")]
        public int Count { get; set; }

    }
}
