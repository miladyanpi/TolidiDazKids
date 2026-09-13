namespace Domain
{
    public class RefreshTokenEntity:Base
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // FK to AspNetUsers.Id
        public string DeviceId { get; set; } = string.Empty; 
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ReplacedByToken { get; set; }
        public string? RemoteIpAddress { get; set; }
        #region Relation
        public virtual Account? Account { get; set; }
        #endregion
    }
}
