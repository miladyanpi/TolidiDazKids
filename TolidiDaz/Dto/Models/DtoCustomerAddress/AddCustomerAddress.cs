using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoCustomerAddress
{
    public class AddCustomerAddress:BaseModel
    {

        [DisplayName("استان")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProvinceID { get; set; }
        [DisplayName("شهر")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CityID { get; set; }
        [DisplayName("کد مشتری")]
        public int? CustomerID { get; set; }
        [DisplayName("آدرس")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Address { get; set; }
        [DisplayName("پلاک")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Plaque { get; set; }
        [DisplayName("واحد")]
        public string? BuildingUnit { get; set; }
        [DisplayName("کد پستی")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? PostalCode { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public bool Default { get; set; }

    }
}
