using Dto.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TolidiAyhan.Constant;
using TolidiAyhan.Services;
using TolidiAyhan.Services.TolidiAyhan.Services;

public class AccountAuthentication : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenServiceServer _tokenService;

    public AccountAuthentication(IHttpContextAccessor httpContextAccessor, TokenServiceServer tokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
    }

    public async Task UpdateAuthenticationState(string token)
    {
        ClaimsPrincipal claimsPrincipal;

        if (!string.IsNullOrWhiteSpace(token))
        {
            var claims = AccountAuthentication.GetClaimsFromToken(token);
            var identity = SetClaimsIdentity(claims);
            claimsPrincipal = new ClaimsPrincipal(identity);

            // ذخیره کوکی یا localStorage
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                AuthenticationConstant.AccessToken,
                token,
                new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.AddDays(7) });
        }
        else
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var token = httpContext?.Request.Cookies[AuthenticationConstant.AccessToken];

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var claims = GetClaimsFromToken(token);
        var identity = SetClaimsIdentity(claims);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private ClaimsIdentity SetClaimsIdentity(UserSession model)
    {
        return new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Role, model.Role),
            new Claim(ClaimTypes.Name, model.NameAndLastName),
            new Claim(ClaimTypes.NameIdentifier, model.UniqCode),
            new Claim(ClaimTypes.PrimarySid, model.CustomerID),
            new Claim(ClaimTypes.UserData, model.UserName),
        }, AuthenticationConstant.Scheme);
    }

    public static UserSession GetClaimsFromToken(string jwtToken)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        var claims = token.Claims;

        string role = claims.First(c => c.Type == ClaimTypes.Role).Value;
        string userName = claims.First(c => c.Type == ClaimTypes.UserData).Value;
        string name = claims.First(c => c.Type == ClaimTypes.Name).Value;
        string customerId = claims.First(c => c.Type == ClaimTypes.PrimarySid).Value;
        string uniqCode = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return new UserSession(role, userName, name, customerId, uniqCode);
    }
}


//using TolidiAyhan.Constant;
//using Dto.DTOs;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.JSInterop;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using Blazored.LocalStorage;
//namespace TolidiAyhan.Services
//{
//    public class AccountAuthentication : AuthenticationStateProvider
//    {

//        private ClaimsPrincipal _user = new(new ClaimsIdentity());
//        private readonly ILocalStorageService _localStorageService;
//        private readonly TokenService _tokenService;

//        public AccountAuthentication(ILocalStorageService localStorageService, TokenService tokenService)
//        {
//            _localStorageService = localStorageService;
//            _tokenService= tokenService;
//        }
//        public async Task UpdateAuthenticationState(string token)
//        {
//            ClaimsPrincipal claimsPrincipal = new();
//            if (!string.IsNullOrWhiteSpace(token))
//            {
//                var authProperties = new AuthenticationProperties
//                {
//                    IsPersistent = false // در صورت نیاز به کوکی پایدار
//                };
//                var claims = GetClaimsFromToken(token);
//                var userClaim = SetClaimsIdentity(claims);
//                claimsPrincipal = new ClaimsPrincipal(userClaim);

//               // await _localStorageService.SetItemAsStringAsync(AuthenticationConstant.TokenTitle, token);
//                await _tokenService.SetToken(token);


//            }
//            else
//            {
//                await _tokenService.ClearToken();
//                claimsPrincipal = _user;
//                //await _localStorageService.RemoveItemAsync(AuthenticationConstant.TokenTitle);

//            }
//            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
//        }

//        private ClaimsIdentity SetClaimsIdentity(UserSession model)
//        {
//            return new ClaimsIdentity(new List<Claim>
//            {
//                new Claim(ClaimTypes.Role,model.Role),
//                new Claim(ClaimTypes.Name,model.NameAndLastName),
//                new Claim(ClaimTypes.NameIdentifier,model.UniqCode),
//                new Claim(ClaimTypes.PrimarySid,model.CustomerID),
//                new Claim(ClaimTypes.UserData,model.UserName),
//            }, AuthenticationConstant.Scheme);
//        }

//        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            try
//            {
//                string Token =await _tokenService.GetToken();

//                if (string.IsNullOrWhiteSpace(Token))
//                    return await Task.FromResult(new AuthenticationState(_user));
//                var claims = GetClaimsFromToken(Token);

//                var ClaimsIdentity = SetClaimsIdentity(claims);
//                var claimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);
//                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
//            }
//            catch
//            {
//                return await Task.FromResult(new AuthenticationState(_user));
//            }
//        }
//        public static UserSession GetClaimsFromToken(string jwtToken)
//        {
//            var handler = new JwtSecurityTokenHandler();
//            var token = handler.ReadJwtToken(jwtToken);
//            var claims = token.Claims;

//            //string Identity = claims.First(c => c.Type == WebAuthorizeConstsClaimType.Identity).Value!;
//            string Role = claims.First(c => c.Type == ClaimTypes.Role).Value!;
//            string UserName = claims.First(c => c.Type == ClaimTypes.UserData).Value!;
//            string NameAndLastName = claims.First(c => c.Type == ClaimTypes.Name).Value!;
//            string CustomerID = claims.First(c => c.Type == ClaimTypes.PrimarySid).Value!;
//            string UniqCode = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;

//            return new UserSession(Role, UserName, NameAndLastName, CustomerID, UniqCode);
//        }
//    }
//}
