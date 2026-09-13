using Dto.Models.Base;
using Dto.Models.DtoProduct_CountAction_CostType;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoPosition
{
    public class ResultPosition : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان شغل")]
        public string? Title { get; set; }
        [DisplayName("نوع هزینه")]
        public string? CostType { get; set; }
        [DisplayName("نوع هزینه")]
        public CostType? CostType2 { get; set; }
        [DisplayName("مبلغ هزینه(تومان)")]
        public Int64 Price { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public List<ResultProduct_CountAction_CostType> ResultProduct_CountAction_CostTypes { get; set; }
    }
}
