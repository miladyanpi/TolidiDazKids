
// ===== RestApiHelper.cs (Refactored) =====
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public class RestApiHelper<T> where T : class
    {
        private string FullUrlApi = string.Empty;
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public RestApiHelper(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public void SetUrl(string url, string endpoint)
        {
            FullUrlApi = Path.Combine(url, endpoint);
        }

        private async Task<HttpRequestMessage> BuildRequest(HttpMethod method, string url, HttpContent? content = null)
        {
            string? token =await _tokenService.GetToken();
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (content != null)
                request.Content = content;

            return request;
        }

        public async Task<HttpResponseMessage> CreatePostRequest(object parameterList)
        {
            var content = GenerateStringContent(SerializeObj(parameterList));
            var request =await BuildRequest(HttpMethod.Post, FullUrlApi, content);
            var res= await _httpClient.SendAsync(request);
            return res; 
        }

        public async Task<HttpResponseMessage> CreatePatchRequest(object parameterList)
        {
            var content = GenerateStringContent(SerializeObj(parameterList));
            var request =await BuildRequest(HttpMethod.Patch, FullUrlApi, content);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> CreateDeleteRequest()
        {
            var request =await BuildRequest(HttpMethod.Delete, FullUrlApi);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> CreateGetRequestAsync(object parameterList)
        {
            string requestUri = FullUrlApi;

            if (parameterList != null)
            {
                string queryString = ConvertToQueryString(parameterList);
                requestUri = $"{FullUrlApi}?{queryString}";
            }

            var request =await BuildRequest(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content)
        {
            var request =await BuildRequest(HttpMethod.Post, FullUrlApi, content);
            return await _httpClient.SendAsync(request);
        }

        private string ConvertToQueryString(object parameterList)
        {
            var json = JsonConvert.SerializeObject(parameterList);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new();
            return string.Join("&", dict.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
        }

        public async Task<DTO> GetContent<DTO>(HttpResponseMessage restResponse)
        {
            var content = await restResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DTO>(content)!;
        }

        public static JsonSerializerOptions JsonOptions() => new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };

        public static string SerializeObj<TModel>(TModel model) => System.Text.Json.JsonSerializer.Serialize(model, JsonOptions());

        public static StringContent GenerateStringContent(string serializedObj) =>
            new(serializedObj, System.Text.Encoding.UTF8, "application/json");
    }
}



////using AdminPanel.Services;
//using AdminPanel.Services;
//using Newtonsoft.Json;
//using System.Net.Http.Headers;
//using System.Text.Json;
//using System.Text.Json.Serialization;


//namespace AdminPanel.Services
//{
//    public class RestApiHelper<T> where T : class
//    {
//        private string FullUrlApi = string.Empty;
//        private readonly HttpClient _httpClient;
//        private readonly TokenService _tokenService;
//        public RestApiHelper(HttpClient httpClient, TokenService tokenService)
//        {
//            _httpClient = httpClient;
//            _tokenService = tokenService; 
//        }

//        public void SetUrl(string url, string endpoint)
//        {
//            FullUrlApi = Path.Combine(url, endpoint);
//        }
//        private void SetAuthorizationHeader()
//        {
//            string? token = _tokenService.GetToken();
//            if (!string.IsNullOrEmpty(token))
//            {

//                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//            }
//            else
//            {
//                _httpClient.DefaultRequestHeaders.Remove("Authorization");
//            }
//        }

//        public async Task<HttpResponseMessage> CreatePostRequest(object parameterList)
//        {
//            SetAuthorizationHeader();
//            HttpResponseMessage res = new HttpResponseMessage();
//            try
//            {
//                res = await _httpClient.PostAsync(FullUrlApi,
//                                   GenerateStringContent(
//                                   SerializeObj(parameterList)));
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            return res;
//        }
//        public async Task<HttpResponseMessage> CreatePatchRequest(object parameterList)
//        {
//            SetAuthorizationHeader();
//            var res = await _httpClient.PatchAsync(FullUrlApi,
//                 GenerateStringContent(
//                 SerializeObj(parameterList)));
//            return res;
//        }
//        public async Task<HttpResponseMessage> CreateDeleteRequest()
//        {
//            SetAuthorizationHeader();
//            var res = await _httpClient.DeleteAsync(FullUrlApi);
//            return res;
//        }

//        public async Task<HttpResponseMessage> CreateGetRequestAsync(object parameterList)
//        {
//            SetAuthorizationHeader(); 
//            string RequestUri = FullUrlApi;
//            if (parameterList != null)
//            {
//                string queryString = ConvertToQueryString(parameterList);
//                RequestUri = $"{FullUrlApi}?{queryString}";
//            }
//            var httpResponseMessage = await _httpClient.GetAsync(RequestUri);
//            return httpResponseMessage;
//        }

//        public async Task<HttpResponseMessage> CreateGetRequestForSearchAsync(object parameterList)
//        {

//            SetAuthorizationHeader();
//            string RequestUri = FullUrlApi;

//            if (parameterList != null)
//            {
//                string queryString = ConvertToQueryString(parameterList);
//                RequestUri = $"{FullUrlApi}?{queryString}";
//            }
//            var httpResponseMessage = await _httpClient.GetAsync(RequestUri);
//            return httpResponseMessage;


//        }
//        public async Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content)
//        {
//            SetAuthorizationHeader();
//            string RequestUri = FullUrlApi;
//            return await _httpClient.PostAsync(FullUrlApi, content);
//        }

//        private string ConvertToQueryString(object parameterList)
//        {
//            var json = JsonConvert.SerializeObject(parameterList);
//            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
//            var queryArray = dictionary!.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}");
//            return string.Join("&", queryArray);
//        }
//        public async Task<DTO> GetContent<DTO>(HttpResponseMessage restResponse)
//        {
//            var content = await restResponse.Content.ReadAsStringAsync();
//            DTO dtoObject = JsonConvert.DeserializeObject<DTO>(content)!;
//            return dtoObject;
//        }
//        public static JsonSerializerOptions JsonOptions()
//        {
//            return new JsonSerializerOptions
//            {
//                AllowTrailingCommas = true,
//                PropertyNameCaseInsensitive = true,
//                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
//            };
//        }
//        public static string SerializeObj<T>(T modelObject) => System.Text.Json.JsonSerializer.Serialize(modelObject, JsonOptions());
//        public static T DeserializeJsonString<T>(string jsonString) => System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
//        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => System.Text.Json.JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

//        public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");

//    }
//}
