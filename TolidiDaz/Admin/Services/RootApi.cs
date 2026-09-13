using Admin.Services.Token;
using Dto.Models.DtoAccount;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;
namespace Admin.Services
{
    public class RootApi<T> : IRootApi<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _TokenService;
        private readonly AuthenticationStateProvider _AuthStateProvider;

        public RootApi(HttpClient httpClient, TokenService tokenService, AuthenticationStateProvider AuthStateProvider)
        {
            _httpClient = httpClient;
            _TokenService = tokenService;
            _AuthStateProvider = AuthStateProvider;
        }
        private async Task<HttpResponseMessage> Run(string endpoint, object parameter, Method method)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TokenService.Token);
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            var FullUrlApi = ApiLink.fullUrl + endpoint;

            switch (method)
            {
                case Method.Post:
                    httpResponseMessage = await _httpClient.PostAsJsonAsync(FullUrlApi, parameter);
                    break;
                case Method.Patch:
                    httpResponseMessage = await _httpClient.PatchAsJsonAsync(FullUrlApi, parameter);
                    break;
                case Method.Delete:
                    httpResponseMessage = await _httpClient.DeleteAsync(FullUrlApi);
                    break;
                case Method.Get:
                    string url = FullUrlApi;

                    if (parameter != null)
                    {
                        var dict = JObject.FromObject(parameter);
                        var qs = string.Join("&", dict.Properties().Select(x =>
                            $"{x.Name}={Uri.EscapeDataString(x.Value!.ToString())}"));

                        url += "?" + qs;
                    }

                    httpResponseMessage = await _httpClient.GetAsync(url);
                    break;
            }
            return httpResponseMessage;
        }
        public async Task<T> RunMethodApi(string endpoint, object parameter, Method method)
        {
            try
            {
                var httpResponseMessage = await Run(endpoint: endpoint, parameter: parameter, method: method);
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    var Result = await _httpClient.GetAsync(
                   $"{ApiLink.fullUrl}auth/RefreshToken?refreshToken={_TokenService.RefreshToken}&DeviceID={_TokenService.DeviceID}");
                    if (Result.IsSuccessStatusCode)
                    {
                        var response = await ReadContent<ResponseApiEntity<ResultLoginAccount>>(Result);
                        _TokenService.SetToken(token: response.Entity.Token, refreshToken: response.Entity.RefreshToken, deviceID: response.Entity.DeviceID);
                        var httpResponseMessage2 = await Run(endpoint: endpoint, parameter: parameter, method: method);
                        return await ReadContent<T>(httpResponseMessage2);
                    }
                    else
                    {
                        await ((AccountAuthentication)_AuthStateProvider).MarkUserAsLoggedOut();
                        _httpClient.DefaultRequestHeaders.Authorization = null;
                        return default(T);
                    }
                }


                var res = await ReadContent<T>(httpResponseMessage);
                return res;
            }
            catch (Exception ex)
            {
                // بهتر: لاگ کردن خطا و شاید پرتاب دوباره خطا
                Console.WriteLine(ex);
                return default(T);
            }
        }
        public async Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method = Method.Post)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _TokenService.Token);

            var FullUrlApi = ApiLink.fullUrl + endpoint;

            var response = method switch
            {
                Method.Post => await _httpClient.PostAsync(FullUrlApi, content),
                Method.Patch => await _httpClient.PatchAsync(FullUrlApi, content),
                _ => null
            };

            if (response == null)
                return default(T);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return default(T);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }


        //public async Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method = Method.Post)
        //{
        //    var FullUrlApi = ApiLink.fullUrl + endpoint;
        //    var response = method switch
        //    {
        //        Method.Post => await _httpClient.PostAsync(FullUrlApi, content),
        //        Method.Patch => await _httpClient.PatchAsync(FullUrlApi, content),
        //        _ => null
        //    };

        //    if (response == null || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        return default(T);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var json = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<T>(json);
        //    }

        //    return default(T);
        //}
        public async Task<TOut?> ReadContent<TOut>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(json);
        }
    }
}


