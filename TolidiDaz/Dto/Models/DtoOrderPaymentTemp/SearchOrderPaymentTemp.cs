using Dto.Models.Base;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoOrderPaymentTemp
{
    public class SearchOrderPaymentTemp : BaseModel
    {
        public int ID { get; set; }
        public string? ResNum { get; set; }
        public string? Amount { get; set; }
        public string? Token { get; set; }
    }
}
