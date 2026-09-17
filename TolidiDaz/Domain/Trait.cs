using System.ComponentModel;

namespace Domain
{
    /// <summary>
    /// ویژگی 
    /// صفت
    /// Feature
    ///  مثلا: "سایز"، "رنگ"، "وزن"
    /// </summary>
    public class Trait:Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<TraitValue>? TraitValues { get; set; }
        public virtual ICollection<CategoryTrait>? CategoryTraits { get; set; } 
    }
}
