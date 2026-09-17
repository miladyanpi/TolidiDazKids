using Dto.Models.DtoUploadFile;
using Newtonsoft.Json;
using RestSharp;

namespace Admin.Services.MageManagementService
{
    public interface IImageManagementByFtpService
    {
        event Action? OnChange;
        List<ResultUploadFile> ResultUploadImages { get; }
        Task AddToListImagesAsync(List<ResultUploadFile> resultUploadFiles);
        Task DeleteImage(List<ResultUploadFile> resultUploadImages);
        Task GetUpdateDataAsync(string guid);
    }

}
