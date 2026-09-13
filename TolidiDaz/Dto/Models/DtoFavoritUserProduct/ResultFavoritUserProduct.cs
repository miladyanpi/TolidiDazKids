using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoFavoritUserProduct
{
    public class ResultFavoritUserProduct : BaseModel
    {
        public int ID { get; set; }

        [DisplayName("کاربر")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CustomerID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        public ResultCustomer? ResultCustomer { get; set; }
        public ResultProduct? ResultProduct { get; set; }

    }
}
