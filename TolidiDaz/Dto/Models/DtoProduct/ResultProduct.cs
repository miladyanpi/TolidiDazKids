using Dto.Models.Base;
using Dto.Models.DtoCategory;
using Dto.Models.DtoPricingRule;
using Dto.Models.DtoUploadFile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProduct
{
    public class ResultProduct : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("دسته بندی سطح 3")]
        public int? CategoryID { get; set; }
        [DisplayName("نام محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("کد محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? ProductCode { get; set; }
        [DisplayName("برند")]
        public string? Brand { get; set; }
        [DisplayName("کد انبارداری(SKU)")]
        public string? SkuCode { get; set; }
        [DisplayName("تخفیف")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Discount { get; set; }
        [DisplayName("وضعیت موجودی")]
        public string? ProductExistStatus { get; set; }//enum ProductExistStatus
        public ProductExistStatus? ProductExistStatus2 { get; set; }//enum ProductExistStatus

        [DisplayName("قیمت پایه محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد موجودی محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Count { get; set; }
        [DisplayName("توضیحات کوتاه")]
        public string? ShortDescription { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPicture { get; set; }
        [DisplayName("تعداد بازدید")]
        public int ViewCount { get; set; }
        [DisplayName("تعداد نظرات")]
        public int ProductCommentCount { get; set; }
        [DisplayName("میانگین امتیاز")]
        public float RatingAverage { get; set; }
        public ResultCategory? ResultCategory { get; set; }
        public List<ResultPricingRule>? ResultPricingRules { get; set; }
        public List<ResultUploadFile>? ResultUploadFiles { get; set; }
    }
}
