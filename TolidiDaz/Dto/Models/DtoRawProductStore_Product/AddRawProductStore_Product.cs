using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoRawProductStore_Product
{
    public class AddRawProductStore_Product:BaseModel
    {
        public int? RawProductStoreID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        [Required(ErrorMessage = "| الزامی است")]
        [DisplayName("تعداد")]
        public int? Count { get; set; }

      
    }
}
