using TolidiAyhan.Constant;
using static TolidiAyhan.Constant.AuthorizeMethodClientServerType;

namespace TolidiAyhan.Services
{
    namespace TolidiAyhan.Services
    {
        public class TokenServiceServerTemp
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public TokenServiceServerTemp(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public Task<string?> GetToken()
            {

                var ctx = _httpContextAccessor.HttpContext;

                if (ctx == null)
                    return Task.FromResult<string?>(null);

                // اول از کوکی بخوان
                if (ctx.Request.Cookies.TryGetValue(AuthenticationConstant.AccessToken, out var tokenFromCookie))
                {
                    return Task.FromResult<string?>(tokenFromCookie);
                }

                // اگر نبود، از Header بخوان
                if (ctx.Request.Headers.TryGetValue("Authorization", out var header))
                {
                    string token = header.ToString().Replace("Bearer ", "");
                    return Task.FromResult<string?>(token);
                }

                return Task.FromResult<string?>(null);
            }


        }
    }

}
