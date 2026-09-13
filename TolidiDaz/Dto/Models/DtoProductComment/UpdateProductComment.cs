using Dto.Models.Base;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProductComment
{
    public class UpdateProductComment : BaseModel
    {
        public int ID { get; set; }
        public int? ProductID { get; set; }
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
        [Required(ErrorMessage = "| الزامی است")]
        public int? Rating { get; set; }
        [DisplayName("وضعیت")]
        [Required(ErrorMessage = "| الزامی است")]
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
        [DisplayName("خریدار")]
        [Required(ErrorMessage = "| الزامی است")]
        public bool IsVerifiedBuyer { get; set; } = false;
        [DisplayName("تعداد لایک")]
        [Required(ErrorMessage = "| الزامی است")]
        public int LikeCount { get; set; } = 0;
        [DisplayName("تعداد دیسلایک")]
        [Required(ErrorMessage = "| الزامی است")]
        public int DislikeCount { get; set; } = 0;


    }
}
