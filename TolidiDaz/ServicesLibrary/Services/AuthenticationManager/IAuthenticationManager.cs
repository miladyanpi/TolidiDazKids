using Domain;
using Dto.Models.DtoAccount;
namespace ServicesLibrary.Services.AuthenticationManagerSrv
{
    public interface IAuthenticationManager
    {
        Task<Account?> ValidateCredentials(LoginAccount credentials);
        Task<string> CreateToken(Account user, bool isRefreshToken);
        Task<bool> CheckTokenIsValid(string token);
        //Task<TokenResponse?> RefreshTokenAsync(string refreshToken, string remoteIp = null);

    }
}
