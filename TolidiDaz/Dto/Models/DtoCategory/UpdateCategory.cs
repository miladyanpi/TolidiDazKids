using Dto.Attributes;
using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCategory
{
    public class UpdateCategory: BaseModel
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        [DisplayName("دسته بندی")]

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

        public ResultCategory? ParentResultCategory { get; set; }


    }
}
