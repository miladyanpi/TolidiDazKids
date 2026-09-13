using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoWallet
{
    public class UpdateWallet : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        public int CustomerID { get; set; }
        [DisplayName("واحد پول")]
        public string Currency { get; set; } = "IRR"; // ISO currency code
        [DisplayName("موجودی")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Balance { get; set; }
        [DisplayName("اعتبار هدیه")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 GiftCredit { get; set; }
    }
}
