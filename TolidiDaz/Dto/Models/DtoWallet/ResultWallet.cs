using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoWallet
{
    public class ResultWallet : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        public int CustomerID { get; set; }
        [DisplayName("واحد پول")]
        public string Currency { get; set; } = "IRR"; // ISO currency code
        [DisplayName("موجودی")]
        public Int64 Balance { get; set; }
        [DisplayName("اعتبار هدیه")]
        public Int64 GiftCredit { get; set; }
    }
}
