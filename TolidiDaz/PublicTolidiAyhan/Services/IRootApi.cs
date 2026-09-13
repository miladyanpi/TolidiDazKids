using RestSharp;

namespace PublicTolidiAyhan.Services
{

    public interface IRootApi<T> where T : class
    {
        Task<T> RunMethodApi(string endpoint, object parameter, Method method);
        Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method = Method.Post);
    }

}
