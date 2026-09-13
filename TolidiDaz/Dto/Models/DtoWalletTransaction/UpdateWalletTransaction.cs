using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoWalletTransaction
{
    public class UpdateWalletTransaction : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("کد کیف پول")]
        public int? WalletID { get; set; }
        [DisplayName("نوع تراکنش")]
        public TransactionKind Kind { get; set; }
        [DisplayName("مبلغ تراکنش")]
        public Int64 Amount { get; set; }
        [DisplayName("موجودی کیف پول")]
        public Int64 AfterBalance { get; set; }
        [DisplayName("واحد پول")]
        public string Currency { get; set; } = "IRR";
        [DisplayName("شناسه ارجاع")]
        public string? RefID { get; set; }
        [DisplayName("شناسه درگاه")]
        public string? ExternalReference { get; set; }
        [DisplayName("شماره کارت")]
        public string? CardPen { get; set; }
        [DisplayName("وضعیت تراکنش")]
        public TransactionStatus Status { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
