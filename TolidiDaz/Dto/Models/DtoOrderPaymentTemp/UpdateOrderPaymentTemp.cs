using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoOrderPaymentTemp
{
    public class UpdateOrderPaymentTemp:BaseModel
    {
        public int ID { get; set; }
        public string? ResNum { get; set; }
        public string? Amount { get; set; }
        public string? Token { get; set; }
    }
}
