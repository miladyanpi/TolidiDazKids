
//// ===== RestApiHelper.cs (Refactored) =====
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//using TolidiAyhan.Services.TolidiAyhan.Services;
//using static TolidiAyhan.Constant.AuthorizeMethodClientServerType;

//namespace TolidiAyhan.Services
//{
//    public class RestApiHelperTemp<T> where T : class
//    {
//        private string FullUrlApi = string.Empty;
//        private readonly HttpClient _httpClient;
//        //private readonly TokenService _tokenService;
//        private readonly TokenServiceServer _tokenService;
//        public RestApiHelperTemp(HttpClient httpClient, TokenServiceServer tokenService)
//        {
//            _httpClient = httpClient;
//            _tokenService = tokenService;
//        }

//        public void SetUrl(string url, string endpoint)
//        {
//            FullUrlApi = Path.Combine(url, endpoint);
//        }
       

//        private async Task<HttpRequestMessage> BuildRequest(HttpMethod method, string url, HttpContent? content = null, string? Token=null)
//        {
//            var request = new HttpRequestMessage(method, url);
          
//            if (!string.IsNullOrEmpty(Token))
//            {
//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
//            }
//            else
//            {
//                string? token = await _tokenService.GetToken();
//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

//            }

//            if (content != null)
//                request.Content = content;

            
//            return request;
//        }

        

//        public async Task<HttpResponseMessage> CreatePostRequest(object parameterList, string? Token)
//        {
//            var content = GenerateStringContent(SerializeObj(parameterList));
//            var request =await BuildRequest(HttpMethod.Post, FullUrlApi, content, Token: Token);
//            var res= await _httpClient.SendAsync(request);
//            return res; 
//        }

//        public async Task<HttpResponseMessage> CreatePatchRequest(object parameterList, string? Token)
//        {
//            var content = GenerateStringContent(SerializeObj(parameterList));
//            var request =await BuildRequest(HttpMethod.Patch, FullUrlApi, content, Token: Token);
//            return await _httpClient.SendAsync(request);
//        }

//        public async Task<HttpResponseMessage> CreateDeleteRequest(string? Token)
//        {
//            var request =await BuildRequest(HttpMethod.Delete, FullUrlApi, Token: Token);
//            return await _httpClient.SendAsync(request);
//        }

//        public async Task<HttpResponseMessage> CreateGetRequestAsync(object parameterList,string? Token)
//        {
//            string requestUri = FullUrlApi;

//            if (parameterList != null)
//            {
//                string queryString = ConvertToQueryString(parameterList);
//                requestUri = $"{FullUrlApi}?{queryString}";
//            }

//            var request =await BuildRequest(HttpMethod.Get, requestUri, Token: Token);
//            return await _httpClient.SendAsync(request);
//        }

//        public async Task<HttpResponseMessage> UploadImage(MultipartFormDataContent content)
//        {
//            var request =await BuildRequest(HttpMethod.Post, FullUrlApi, content);
//            return await _httpClient.SendAsync(request);
//        }

//        private string ConvertToQueryString(object parameterList)
//        {
//            var json = JsonConvert.SerializeObject(parameterList);
//            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new();
//            return string.Join("&", dict.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
//        }

//        public async Task<DTO> GetContent<DTO>(HttpResponseMessage restResponse)
//        {
//            var content = await restResponse.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<DTO>(content)!;
//        }

//        public static JsonSerializerOptions JsonOptions() => new()
//        {
//            AllowTrailingCommas = true,
//            PropertyNameCaseInsensitive = true,
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
//        };

//        public static string SerializeObj<TModel>(TModel model) => System.Text.Json.JsonSerializer.Serialize(model, JsonOptions());

//        public static StringContent GenerateStringContent(string serializedObj) =>
//            new(serializedObj, System.Text.Encoding.UTF8, "application/json");
//    }
//}


