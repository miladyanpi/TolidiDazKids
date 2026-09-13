
using Dto.Attributes;
using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoProduct
{
    public class AddProduct : BaseModel
    {
        [DisplayName("دسته بندی سطح 3")]
        public int? CategoryID { get; set; }
        [DisplayName("نام محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }

        [DisplayName("کد محصول")]
        public string? ProductCode { get; set; }
        [DisplayName("برند")]
        public string? Brand { get; set; }
        [DisplayName("کد انبارداری(SKU)")]
        public string? SkuCode { get; set; }
        [DisplayName("تخفیف")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Discount { get; set; }
        [DisplayName("وضعیت موجودی")]
        public ProductExistStatus? ProductExistStatus { get; set; }//enum ProductExistStatus

        [DisplayName("قیمت پایه محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد موجودی محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Count { get; set; }
        [DisplayName("توضیحات کوتاه")]
        public string? ShortDescription { get; set; }
        [DisplayName("توضیحات کامل")]
        public string? Description { get; set; }
        [DisplayName("تصویر")]
        public string? JsonPicture { get; set; }
  

    }
}
