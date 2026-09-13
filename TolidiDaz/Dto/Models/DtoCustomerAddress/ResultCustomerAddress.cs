using Dto.Models.Base;
using Dto.Models.DtoCity;
using Dto.Models.DtoCustomer;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoCustomerAddress
{
    public class ResultCustomerAddress : BaseModel
    {
        public int ID { get; set; }

        [DisplayName("شهر")]
        public int? CityID { get; set; }
        [DisplayName("کد مشتری")]
        public int? CustomerID { get; set; }
        [DisplayName("آدرس")]
        public string? Address { get; set; }
        [DisplayName("پلاک")]
        public string? Plaque { get; set; }
        [DisplayName("واحد")]
        public string? BuildingUnit { get; set; }
        [DisplayName("کد پستی")]
        public string? PostalCode { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public bool Default { get; set; }
        public ResultCustomer? ResultCustomer { get; set; }
        public ResultCity? ResultCity { get; set; }

    }
}
