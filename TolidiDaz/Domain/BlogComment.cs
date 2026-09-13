using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace Domain
{
    public class BlogComment : Base
    {
        public int? BlogID { get; set; }
        // اگر کاربر عضو باشد
        public int? CustomerID { get; set; }
        public int? ParentID { get; set; }
        // اگر مهمان باشد
        public string? AuthorName { get; set; }
        public string? Body { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
        /// <summary>
        /// کاربری که در حال کامنت گذاری است قبلا این محصول را خریده است
        /// </summary>
        public bool IsVerifiedBuyer { get; set; } = false;
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;

        #region RelationShip
        public virtual Blog? Blog { get; set; }
        public virtual Customer? Customer { get; set; }
        #endregion
    }

}
