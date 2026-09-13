using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtoStory
{
    public class StoryItem
    {
        
        public string? Type { get; set; }
        public string? User { get; set; }
        public string? Avatar { get; set; }
        public string? Url { get; set; }
        public int? Duration { get; set; }
        public string? Link { get; set; }
    }

}
