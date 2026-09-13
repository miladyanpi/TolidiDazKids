using RestSharp;

namespace AdminPanel.Services
{
    public interface IRootApi<T> where T : class
    {
        Task<T> RunMethodApi(string endpoint, object? parameterList, Method method);
        Task<T> UploadImage(string endpoint, MultipartFormDataContent content, Method method);
    }
}
