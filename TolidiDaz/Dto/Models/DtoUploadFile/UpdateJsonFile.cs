using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoUploadFile
{
    public class UpdateJsonFile 
    {
        public int ID { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("فایل")]
        public string? JsonFile { get; set; }

        [DisplayName("ویدئو")]
        public string? JsonVideo { get; set; }

        public EnumJsonImageFileVideo EnumJsonImageFileVideo { get; set; }


    }
}
