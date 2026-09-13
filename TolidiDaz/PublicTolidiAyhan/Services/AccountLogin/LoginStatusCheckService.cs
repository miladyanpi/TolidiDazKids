using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;

namespace PublicTolidiAyhan.Services
{
    public class LoginStatusCheckService(AuthenticationStateProvider _AuthStateProvider)
    {
        public async Task<(bool IsLoggedIn,string Name,string UserName,string Role)> GetLoginStatusCheck()
        {
            string name = string.Empty, userName = string.Empty;
            string role = string.Empty;
            var authState = await _AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            bool isLoggedIn = false;
            var IsLoggedIn = user.Identity?.IsAuthenticated ?? false;
            if (IsLoggedIn)
            {
                name = user.Claims.First(s => s.Type == System.Security.Claims.ClaimTypes.Name).Value;
                userName = user.Claims.First(s => s.Type == System.Security.Claims.ClaimTypes.UserData).Value;
                role = user.Claims.First(s => s.Type == System.Security.Claims.ClaimTypes.Role).Value;
                isLoggedIn=true;
            }
            return (isLoggedIn, name, userName, role);
        }
    }
}
