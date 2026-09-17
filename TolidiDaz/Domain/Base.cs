using Microsoft.EntityFrameworkCore.Update.Internal;
using Utility;

namespace Domain
{

    public class Base
    {
        public Base()
        {
            IdentityCode = Guid.NewGuid();
            Visible = true;
        }

        public int ID { get; set; }
        public Guid? IdentityCode { get; set; }
        public int RegisterDate { get; set; }
        public TimeSpan RegisterTime { get; set; }
        public int EditDate { get; set; } 
        public TimeSpan EditTime { get; set; }
        public string? JsonLableTexts { get; set; }
        public bool Visible { get; set; }
        public string? Slug { get; set; }

    }
}
