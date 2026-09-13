using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoRefreshTokenEntity
{
    public class ResultRefreshTokenEntity:BaseModel
    {
        public int ID { get; set; }

        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // FK to AspNetUsers.Id
        public string DeviceId { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReplacedByToken { get; set; }
        public string? RemoteIpAddress { get; set; }
    }
}
