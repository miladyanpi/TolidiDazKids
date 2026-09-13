using static Dto.Enum.EnumConstant;

namespace Domain
{
    public class PricingRule:Base
    {
        public string? Title { get; set; }
        public int? ProductID { get; set; }
        /// <summary>
        /// id مربوط نقش کاربر در جدول AspNetUserRoles را دراینجا ذخیره کنید
        /// </summary>
        public string? RoleID { get; set; }
        public int? RuleType { get; set; }
        public int MinQuantity { get; set; } = 1;  // حداقل تعداد برای اعمال قانون
        public Int64 Price { get; set; }
        /// <summary>
        /// 
        /// این دوتاریخ زمانی استفاده میشه که
        /// نوع RuleType از نوع Date باشه برای جشنواره  وغیره
        /// </summary>
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        #region Relation
        public virtual Product? Product { get; set; }
        #endregion
    }
}
