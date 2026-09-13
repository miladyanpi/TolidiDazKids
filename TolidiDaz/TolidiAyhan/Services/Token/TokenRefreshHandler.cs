using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class TokenRefreshHandler : DelegatingHandler
{
    private readonly TokenServiceServer _tokenService;
    private readonly NavigationManager _navigation;

    public TokenRefreshHandler(TokenServiceServer tokenService, NavigationManager navigation)
    {
        _tokenService = tokenService;
        _navigation = navigation;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GetAccessToken();

        // --- نکته مهم ---
        // اگر توکن نداریم → درخواست را همانطور که هست بفرست (برای API های پابلیک)
        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        // اگر نیاز به احراز هویت نبوده → کاری نکن
        if (string.IsNullOrEmpty(accessToken))
            return response;

        // اگر احراز هویت شده‌ایم ولی 401 برگشته → توکن منقضی شده
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshToken = _tokenService.GetRefreshToken();

            // اگر رفرش نیست → کاربر باید لاگین کند
            if (string.IsNullOrEmpty(refreshToken))
            {
                LogoutAndRedirect();
                return response;
            }

            // درخواست واقعی رفرش توکن
            var newTokens = await _tokenService.RefreshTokensAsync(refreshToken);

            if (newTokens == null)
            {
                LogoutAndRedirect();
                return response;
            }

            // ذخیره‌سازی توکن‌های جدید
            _tokenService.SaveTokens(newTokens.AccessToken, newTokens.RefreshToken);

            // ارسال مجدد همان درخواست
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task LogoutAndRedirect()
    {
        await _tokenService.RemoveTokensAsync();
        _navigation.NavigateTo("/Login", forceLoad: true);
    }
}


//using Microsoft.AspNetCore.Components;
//using System.Net.Http.Headers;

//public class TokenRefreshHandler : DelegatingHandler
//{
//    private readonly TokenServiceServer _tokenService;
//    private readonly NavigationManager _navigation;
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public TokenRefreshHandler(TokenServiceServer tokenService, NavigationManager navigation, IHttpContextAccessor httpContextAccessor)
//    {
//        _tokenService = tokenService;
//        _navigation = navigation;
//        _httpContextAccessor = httpContextAccessor;
//    }

//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var token = _tokenService.GetAccessToken();
//        if (!string.IsNullOrEmpty(token))
//        {
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//        }

//        var response = await base.SendAsync(request, cancellationToken);

//        // اگر 401 آمد → سعی کن رفرش کنی
//        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//        {
//            var refreshSuccess =  _tokenService.GetRefreshToken();
//            if (string.IsNullOrEmpty(refreshSuccess))
//            {
//                // دوباره درخواست رو با توکن جدید بفرست
//                token = _tokenService.GetAccessToken();
//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                response = await base.SendAsync(request, cancellationToken);
//            }
//            else
//            {
//                // رفرش هم نشد → کاربر رو بنداز بیرون
//                _tokenService.RemoveTokens();

//                // مهم: در Blazor Server از NavigationManager استفاده کن
//                _navigation.NavigateTo("/Login?returnUrl=" + Uri.EscapeDataString(_navigation.Uri), forceLoad: true);

//                // یا اگر از HttpContext استفاده می‌کنی (در بعضی موارد کار می‌کنه)
//                // var context = _httpContextAccessor.HttpContext;
//                // context?.Response.Redirect("/Login");
//            }
//        }

//        return response;
//    }
//}