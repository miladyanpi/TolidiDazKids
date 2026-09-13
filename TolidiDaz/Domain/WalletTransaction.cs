using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Dto.Enum.EnumConstant;

namespace Domain
{
    public class WalletTransaction:Base
    {
        public int? WalletID { get; set; }
        public TransactionKind Kind { get; set; }
        public Int64 Amount { get; set; }         // همیشه مثبت
        public Int64 AfterBalance { get; set; }         // موجودی کیف پول بعد از تراکنش
        public string Currency { get; set; } = "IRR"; // باید با والت همخوانی داشته باشد
        public string? RefID { get; set; }       // شناسه داخلی یا مرجع تراکنش (مثلاً order id)
        public string? CardPen { get; set; }
        public string? ExternalReference { get; set; } // شناسهٔ درگاه/بانک
        public TransactionStatus Status { get; set; } 
        public string? Description { get; set; }
        #region RelationShip
        public virtual Wallet? Wallet { get; set; }
        #endregion
    }
}
