using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Extensions.Hosting;

namespace Domain
{
    public class Category:Base
    {
        public int? ParentID { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? JsonPicture { get; set; }
        public int Order { get; set; }

        #region RelationShip
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<ProductFeature>? ProductFeatures { get; set; }
        public virtual ICollection<CategoryTrait>? CategoryTraits { get; set; }
        #endregion
    }
}
