using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoLogin
{
    public class AuthCredentials
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
