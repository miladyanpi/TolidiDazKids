using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoCategoryTrait
{
    public class UpdateCategoryTrait:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه دسته‌بندی")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CategoryID { get; set; }
        [DisplayName("شناسه ویژگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? TraitID { get; set; }

    }
}
