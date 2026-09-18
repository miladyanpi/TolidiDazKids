using Dto.Models.Base;
using Dto.Models.DtoCategory;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductVariant;
using Dto.Models.DtoTrait;
using Dto.Models.DtoTraitValue;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoCategoryTrait
{
    public class ResultCategoryTrait : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه دسته‌بندی")]
        public int? CategoryID { get; set; }
        [DisplayName("شناسه ویژگی")]
        public int? TraitID { get; set; }
        public ResultCategory? ResultCategory { get; set; }
        public ResultTrait? ResultTrait { get; set; }

    }
}
