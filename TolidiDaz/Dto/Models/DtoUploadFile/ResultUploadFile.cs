using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Dto.Models.DtoUploadFile
{
    public class ResultUploadFile
    {
        [DisplayName("عنوان")]
        public string? Title { get; set; }
        [DisplayName("فایل")]
        public string? PathFileName { get; set; }
    }
}
