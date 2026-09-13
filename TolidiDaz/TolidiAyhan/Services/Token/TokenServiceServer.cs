using Dto.Models.DtoAccount;
using Microsoft.JSInterop;
using TolidiAyhan.Constant;
using TolidiAyhan.Services;

public class TokenServiceServer
{
    private readonly IHttpContextAccessor _context;
    private readonly IJSRuntime _jsRuntime;

    public TokenServiceServer(IHttpContextAccessor context, IJSRuntime jsRuntime)
    {
        _context = context;
        _jsRuntime = jsRuntime;
    }

    public string? GetAccessToken() =>
        _context.HttpContext?.Request.Cookies[AuthenticationConstant.AccessToken];

    public string? GetRefreshToken() =>
        _context.HttpContext?.Request.Cookies[AuthenticationConstant.RefreshToken];

    public void SaveTokens(string accessToken, string refreshToken)
    {
        var ctx = _context.HttpContext;
        if (ctx == null) return;

        ctx.Response.Cookies.Append(AuthenticationConstant.AccessToken, accessToken, new CookieOptions { HttpOnly = true });
        ctx.Response.Cookies.Append(AuthenticationConstant.RefreshToken, refreshToken, new CookieOptions { HttpOnly = true });
    }

    //public void RemoveTokens()
    //{
    //    var ctx = _context.HttpContext;
    //    if (ctx == null) return;

    //    ctx.Response.Cookies.Delete(AuthenticationConstant.AccessToken);
    //    ctx.Response.Cookies.Delete(AuthenticationConstant.RefreshToken);
    //}
    public async Task RemoveTokensAsync()
    {
        // فقط در کلاینت کوکی‌ها را پاک کن
        await _jsRuntime.InvokeVoidAsync("eval",
            @"document.cookie = 'access_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
          document.cookie = 'refresh_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';");
    }
    // متد واقعی رفرش
    public async Task<TokenResponse?> RefreshTokensAsync(string refreshToken)
    {
        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync(ApiLink.RefreshTokenUrl, new { refreshToken });

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }
}
