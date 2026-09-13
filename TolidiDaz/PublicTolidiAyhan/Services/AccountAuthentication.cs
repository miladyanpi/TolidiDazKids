using PublicTolidiAyhan.Constant;
using PublicTolidiAyhan.Services.Token;
using Dto.Models.DtoAccount;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Utility;

namespace PublicTolidiAyhan.Services
{
    public sealed class AccountAuthentication : AuthenticationStateProvider
    {
        private readonly EncryptionMethodService _encryptionMethodService;
        private readonly ProtectedLocalStorage _LocalStorage;
        private readonly TokenService _tokenService;


        private static readonly ClaimsPrincipal _Anonymous = new(new ClaimsIdentity());

        public AccountAuthentication(
            ProtectedLocalStorage localStorage,
              EncryptionMethodService encryptionMethodService,
              TokenService tokenService)
        {
            _LocalStorage = localStorage;
            _encryptionMethodService = encryptionMethodService;
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var storedUser = await _LocalStorage.GetAsync<string>(AuthenticationConstant.CookiState);
                if (!storedUser.Success || string.IsNullOrWhiteSpace(storedUser.Value))
                    return new AuthenticationState(_Anonymous);

                var decryptedJson = _encryptionMethodService.DecryptString(storedUser.Value);
                var model = JsonSerializer.Deserialize<ResultLoginAccount>(decryptedJson);
                _tokenService.SetToken(token: model.Token, refreshToken: model.RefreshToken, deviceID: model.DeviceID);
                var principal = GetClaimsIdentity(model.Token);
                return new AuthenticationState(
                    new ClaimsPrincipal(principal));
            }
            catch
            {
                return new AuthenticationState(_Anonymous);
            }
        }
        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtoken = handler.ReadJwtToken(token);
            return new ClaimsIdentity(jwtoken.Claims, "jwt");
        }
        public async Task MarkUserAsAuthenticated(ResultLoginAccount model)
        {
            var json = JsonSerializer.Serialize(model);
            var encryptedJson = _encryptionMethodService.EncryptString(json);
            await _LocalStorage.SetAsync(AuthenticationConstant.CookiState, encryptedJson);

            var identity = GetClaimsIdentity(model.Token);
            _tokenService.SetToken(model.Token, model.RefreshToken, model.DeviceID);
            var principal = new ClaimsPrincipal(identity);
            //await _httpContextAccessor.HttpContext.SignInAsync(
            //CookieAuthenticationDefaults.AuthenticationScheme,
            //principal);
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            var Result = await _LocalStorage.GetAsync<string>(AuthenticationConstant.CookiState);
            if (Result.Success || !string.IsNullOrWhiteSpace(Result.Value))
                await _LocalStorage.DeleteAsync(AuthenticationConstant.CookiState);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_Anonymous)));
        }
        public async Task<string?> GetTokenString()
        {

            var storedUser = await _LocalStorage.GetAsync<string>(AuthenticationConstant.CookiState);
            if (!storedUser.Success || string.IsNullOrWhiteSpace(storedUser.Value))
                return null;
            var decryptedJson = _encryptionMethodService.DecryptString(storedUser.Value);
            var model = JsonSerializer.Deserialize<ResultLoginAccount>(decryptedJson);
            return model?.Token;

        }
    }
}