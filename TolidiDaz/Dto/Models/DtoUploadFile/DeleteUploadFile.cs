using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Dto.Models.DtoUploadFile
{
    public class DeleteUploadFile
    {

        public int? ID { get; set; }
        public List<ResultUploadFile>? ResultUploadFiles { get; set; }
    }
}
