using Dto.Models.Base;
using Dto.Models.DtoPersonel;
using Dto.Models.DtoRawProductStore_Product;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoRegisterCostRawProductStore
{
    public class UpdateRegisterCostRawProductStore:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصولات تولیدی")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? RawProductStore_ProductID { get; set; }
        [DisplayName("کارکنان")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? PersonelID { get; set; }
        [DisplayName("تعداد")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Count { get; set; }
        [DisplayName("مبلغ")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("مبلغ کل(تومان)")]
        public Int64 SumPrice { get; set; }
        public ResultPersonel? ResultPersonel { get; set; }
        public ResultRawProductStore_Product? ResultRawProductStore_Product { get; set; }
    }
}
