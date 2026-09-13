using Dto.Models.DtoUploadFile;

namespace Admin.Services.MageManagementService
{
    public interface IImageManagementWithoutFtpService
    {
        string? Guid { get; set; }
        event Action? OnChange;
        List<ResultUploadFile> ResultUploadImages { get; set; }
        Task AddToListImagesAsync(List<ResultUploadFile> resultUploadFiles);
        Task DeleteImage( List<ResultUploadFile> resultUploadImages);
        Task GetUpdateDataAsync();
    }

}
