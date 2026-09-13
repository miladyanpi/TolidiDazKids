using Microsoft.AspNetCore.Http;

namespace ServicesLibrary.Services.StorageSrv
{
    public interface IStorageService
    {
        int Upload(List<IFormFile> fromFiles);
    }
}
