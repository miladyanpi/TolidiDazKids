using Dto.Models.Base;
using Dto.Models.DtoProduct;
using Dto.Models.DtoRawProductStore;
using System.ComponentModel;

namespace  Dto.Models.DtoRawProductStore_Product
{
    public class ResultRawProductStore_Product : BaseModel
    {
        public int ID { get; set; }
        public int? RawProductStoreID { get; set; }
        public int? ProductID { get; set; }
        [DisplayName("تعداد")]
        public int? Count { get; set; }

        public ResultProduct? ResultProduct { get; set; }
    }
}
