using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Dto.Models.DtoUploadFile
{
    public class ResultDownloadFile
    {
        [DisplayName("پسوند فایل")]
        public string? FileExtention { get; set; }

        [DisplayName("فایل")]
        public byte[]? Content { get; set; }

    }
}
