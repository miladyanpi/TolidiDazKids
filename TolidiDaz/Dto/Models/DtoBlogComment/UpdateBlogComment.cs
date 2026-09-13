using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoBlogComment
{
    public class UpdateBlogComment : BaseModel
    {
        public int ID { get; set; }
        public int? BlogID { get; set; }
        // اگر کاربر عضو باشد
        public int? CustomerID { get; set; }
        public int? ParentID { get; set; }

        // اگر مهمان باشد
        [DisplayName("نام نویسنده")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? AuthorName { get; set; }
        [DisplayName("پیام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Body { get; set; }
        [Range(1, 5)]
        [DisplayName("امتیاز")]
        public int? Rating { get; set; }
        [DisplayName("وضعیت")]
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
        [DisplayName("خریدار")]
        public bool IsVerifiedBuyer { get; set; } = false;
        [DisplayName("تعداد لایک")]
        public int LikeCount { get; set; } = 0;
        [DisplayName("تعداد دیسلایک")]
        public int DislikeCount { get; set; } = 0;


    }
}
