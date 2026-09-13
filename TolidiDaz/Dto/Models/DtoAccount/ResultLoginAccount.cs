using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Dto.Models.DtoAccount
{
    public class ResultLoginAccount
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceID { get; set; }
        public long? TokenExpired { get; set; }

    }
}
