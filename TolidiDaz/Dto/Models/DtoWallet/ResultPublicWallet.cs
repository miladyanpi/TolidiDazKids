using Dto.Models.Base;
using Dto.Models.DtoWalletTransaction;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoWallet
{
    public class ResulPublitWallet 
    {

        [DisplayName("موجودی")]
        public Int64 Balance { get; set; }
        [DisplayName("اعتبار هدیه")]
        public Int64 GiftCredit { get; set; }
        public ICollection<ResultWalletTransaction>? ResultWalletTransaction { get; set; }
    }
}
