using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoProductComment;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoProductComment
{
    public class ResultProductComment : BaseModel
    {
        public int ID { get; set; }
        public int? ProductID { get; set; }
        // اگر کاربر عضو باشد
        public int? CustomerID { get; set; }
        public int? ParentID { get; set; }
        // اگر مهمان باشد
        [DisplayName("نام نویسنده")]
        public string? AuthorName { get; set; }
        [DisplayName("پیام")]
        public string? Body { get; set; }
        [DisplayName(" امتیاز ")]
        public int? Rating { get; set; }
        [DisplayName("وضعیت")]
        //CommentStatus
        public string? Status { get; set; } 
        public CommentStatus? Status2 { get; set; } 
        [DisplayName("خریدار")]
        public string? IsVerifiedBuyer { get; set; }
        [DisplayName("تعداد لایک")]
        public int LikeCount { get; set; } = 0;
        [DisplayName("تعداد دیسلایک")]
        public int DislikeCount { get; set; } = 0;
        [DisplayName("نام محصول")]
        public ResultProduct? ResultProduct { get; set; }
        [DisplayName("مشتری")]
        public ResultCustomer? ResultCustomer { get; set; }




    }
}
