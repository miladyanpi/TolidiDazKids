using AdminPanel.Services;
using RestSharp;
public class RootApi<T> : IRootApi<T> where T : class
{
    private readonly TokenService _tokenService;
    private readonly HttpClient _httpClient;


    public RootApi(TokenService tokenService, HttpClient httpClient)
    {
        _tokenService = tokenService;
        _httpClient = httpClient;
    }


    public async Task<T> RunMethodApi(string endpoint, object parameterList, Method method)
    {
        var user = new RestApiHelper<T>(_httpClient, _tokenService);
        user.SetUrl(ApiLink.fullUrl, endpoint);


        HttpResponseMessage response = method switch
        {
            Method.Post => await user.CreatePostRequest(parameterList!),
            Method.Patch => await user.CreatePatchRequest(parameterList),
            Method.Delete => await user.CreateDeleteRequest(),
            Method.Get => await user.CreateGetRequestAsync(parameterList),
            _ => await user.CreateGetRequestAsync(parameterList)
        };


        var q= await user.GetContent<T>(response);
        return q;
    }


    public async Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method)
    {
        var user = new RestApiHelper<T>(_httpClient, _tokenService);
        user.SetUrl(ApiLink.fullUrl, endpoint);


        HttpResponseMessage response = method switch
        {
            Method.Post => await user.UploadImage(content),
            _ => await user.UploadImage(content)
        };


        return await user.GetContent<T>(response);
    }
}




////using AdminPanel.Services;
//using AdminPanel.Services;
//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components.Forms;
//using RestSharp;

//namespace AdminPanel.Services
//{
//    public class RootApi<T> : IRootApi<T> where T : class
//    {
//        private readonly TokenService _tokenService; // تزریق TokenService
//        private readonly HttpClient _httpClient;
//        public RootApi(TokenService tokenService, HttpClient httpClient)
//        {
//            _tokenService = tokenService;
//            _httpClient = httpClient;

//        }
//        public  async Task<T> RunMethodApi(string endpoint, object parameterList, Method method)
//        {
//            var user = new RestApiHelper<T>(_httpClient, _tokenService);
//            user.SetUrl(ApiLink.fullUrl, endpoint);

//            HttpResponseMessage response=new HttpResponseMessage();
//            switch (method)
//            {
//                case Method.Post:
//                    response = await user.CreatePostRequest(parameterList!);
//                    break;
//                case Method.Patch:
//                      response =await user.CreatePatchRequest(parameterList);
//                    break;
//                case Method.Put:
//                    //   response = user.CreatePutRequest(parameterList, token);
//                    break;
//                case Method.Delete:
//                    response =await user.CreateDeleteRequest();
//                    break;
//                case Method.Get:
//                    response = await user.CreateGetRequestAsync(parameterList);
//                    break;
//                default:
//                    response = await user.CreateGetRequestAsync(parameterList);
//                    break;
//            }
          
     

//            T content = await user.GetContent<T>(response);
//            return content;

//        }
//        public async Task<T> UploadImage(string endpoint,  MultipartFormDataContent content, Method method)
//        {
//            var user = new RestApiHelper<T>(_httpClient, _tokenService);
//            user.SetUrl(ApiLink.fullUrl, endpoint);

//            HttpResponseMessage response = new HttpResponseMessage();
//            switch (method)
//            {
//                case Method.Post:
//                    response = await user.UploadImage(content);
//                    break;
                
//                default:
//                    response = await user.UploadImage(content);
//                    break;
//            }



//            T Resultcontent = await user.GetContent<T>(response);
//            return Resultcontent;

//        }
//    }
//}
