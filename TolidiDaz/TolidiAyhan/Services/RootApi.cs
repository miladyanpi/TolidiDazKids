using Newtonsoft.Json;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using TolidiAyhan.Exceptions;
using TolidiAyhan.Services;

public class RootApi<T>  where T : class
{
    private readonly TokenServiceServer _tokenService;
    private readonly HttpClient _http;

    public RootApi(TokenServiceServer tokenService, HttpClient http)
    {
        _tokenService = tokenService;
        _http = http;
    }

    private bool IsTokenValid(string? token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.ValidTo > DateTime.UtcNow.AddSeconds(30);
        }
        catch
        {
            return false;
        }
    }

    public async Task<(T? Result, bool Unauthorized)> RunMethodApi(
        string endpoint,
        object parameter,
        Method method,string? Token=null)
    {
        // دریافت توکن
        var token = Token!=null? Token: _tokenService.GetAccessToken();

        // اگر توکن دارید، بررسی اعتبار
        if (!string.IsNullOrEmpty(token))
        {
            if (!IsTokenValid(token))
            {
                var refreshResult = await _tokenService.RefreshTokensAsync(_tokenService.GetRefreshToken());
                if (refreshResult != null)
                    _tokenService.SaveTokens(refreshResult.AccessToken, refreshResult.RefreshToken);

                token = refreshResult?.AccessToken;
            }
        }

        // اگر توکن هنوز معتبر بود → روی HttpClient ست کن
        if (!string.IsNullOrEmpty(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var helper = new RestApiHelper<T>(_http);
        helper.SetUrl(ApiLink.fullUrl, endpoint);

        HttpResponseMessage response = method switch
        {
            Method.Post => await helper.Post(parameter),
            Method.Patch => await helper.Patch(parameter),
            Method.Delete => await helper.Delete(),
            Method.Get => await helper.Get(parameter),
            _ => await helper.Get(parameter)
        };
        var t = string.IsNullOrEmpty(token);
        // اگر Endpoint نیازمند توکن بود و 401 برگشت:
        if (response.StatusCode == HttpStatusCode.Unauthorized )
        {
            return (default, false);
        }
        else
        {
            var content = await helper.ReadContent<T>(response);
            return (content, true);
        }
         
    }
}

//using RestSharp;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net.Http.Headers;
//using TolidiAyhan.Services;
//using TolidiAyhan.Services.TolidiAyhan.Services;
//using static TolidiAyhan.Constant.AuthorizeMethodClientServerType;
//public class RootApi<T> : RootApi<T> where T : class
//{
//    private readonly TokenServiceServer _tokenService;
//    private readonly HttpClient _httpClient;
//    private readonly IHttpContextAccessor _contextAccessor;



//    public RootApi(TokenServiceServer tokenService, HttpClient httpClient, IHttpContextAccessor contextAccessor)
//    {
//        _tokenService = tokenService;
//        _httpClient = httpClient;
//        _contextAccessor = contextAccessor;


//    }
//    public async Task<bool> CheckTokenIsValid(string token)
//    {
//        try
//        {
//            var jwtSecurityToken = new JwtSecurityToken(token);
//            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public async Task<T> RunMethodApi(string endpoint, object parameterList, Method method, string? Token = null)
//    {
//        var x = string.IsNullOrEmpty(Token);
//        if (x)
//        {
//            Token = await _tokenService.GetToken();
//        }
//        if (!x)
//        {
//            bool isValid = await CheckTokenIsValid(Token);

//            if (!isValid)
//            {
//                _contextAccessor.HttpContext!.Response.Redirect("/Login");

//                //throw new UnauthorizedAccessException("دویاره وارد سیستم شوید");
//            }
//        }
//        var user = new RestApiHelper<T>(_httpClient, _tokenService);
//        user.SetUrl(ApiLink.fullUrl, endpoint);


//        HttpResponseMessage response = method switch
//        {
//            Method.Post => await user.CreatePostRequest(parameterList!, Token),
//            Method.Patch => await user.CreatePatchRequest(parameterList, Token),
//            Method.Delete => await user.CreateDeleteRequest(Token),
//            Method.Get => await user.CreateGetRequestAsync(parameterList, Token),
//            _ => await user.CreateGetRequestAsync(parameterList, Token)
//        };


//        var q = await user.GetContent<T>(response);
//        return q;
//    }


//    public async Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method)
//    {
//        var user = new RestApiHelper<T>(_httpClient, _tokenService);
//        user.SetUrl(ApiLink.fullUrl, endpoint);


//        HttpResponseMessage response = method switch
//        {
//            Method.Post => await user.UploadImage(content),
//            _ => await user.UploadImage(content)
//        };


//        return await user.GetContent<T>(response);
//    }
//}
