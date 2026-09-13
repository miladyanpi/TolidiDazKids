using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoRegisterCostRawProductStore
{
    public class AddRegisterCostRawProductStore:BaseModel
    {

        [DisplayName("محصولات تولیدی")]
        //[Required(ErrorMessage = "| الزامی است")]
        public int? RawProductStore_ProductID { get; set; }
        [DisplayName("کارکنان")]
        //[Required(ErrorMessage = "| الزامی است")]
        public int? PersonelID { get; set; }
        [DisplayName("تعداد")]
        //[Required(ErrorMessage = "| الزامی است")]
        public int Count { get; set; }
        [DisplayName("مبلغ(تومان)")]
        //[Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("مبلغ کل(تومان)")]
        public Int64 SumPrice { get; set; }

    }
}
