using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCategory
{
    public class ResultCategory : BaseModel
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        [DisplayName("عنوان دسته بندی")]
        public string? Title { get; set; }
        [DisplayName("اسلاگ")]
        public string? Slug { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPicture { get; set; }
        [DisplayName("ترتیب")]
        public int Order { get; set; }
        [DisplayName("تعداد زیردسته")]
        public int Count { get; set; }
        public  List<ResultUploadFile>? ResultUploadFiles { get; set; }
        public List<ResultCategory>? ResultCategorys { get; set; }=new List<ResultCategory>();  
    }
}
