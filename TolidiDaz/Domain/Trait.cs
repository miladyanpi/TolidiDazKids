using System.ComponentModel;
using static Dto.Enum.EnumConstant;

namespace Domain
{
    /// <summary>
    /// ویژگی 
    /// Feature
    ///  مثلا: "سایز"، "رنگ"، "وزن"
    /// </summary>
    public class Trait:Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TraitDisplayType DisplayType { get; set; } 
        public virtual ICollection<TraitValue>? TraitValues { get; set; }
        public virtual ICollection<CategoryTrait>? CategoryTraits { get; set; } 
    }
}
