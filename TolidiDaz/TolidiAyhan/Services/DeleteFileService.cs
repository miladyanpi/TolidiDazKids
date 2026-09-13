using Dto.Models.DtoSetting;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Newtonsoft.Json;
using RestSharp;
using TolidiAyhan.Services;
using TolidiAyhan.Services.TolidiAyhan.Services;

namespace TolidiAyhan.Services
{
    public class DeleteFileService : IDeleteFileService
    {
        private readonly TokenServiceServer _tokenService;
        private readonly RootApi<ResponseApiEntity<UpdateSetting>> _settingApi;
        private readonly RootApi<ResponseApiEntities<ResultUploadFile>> _fileDeleteApi;

        public DeleteFileService(
            TokenServiceServer tokenService,
            RootApi<ResponseApiEntity<UpdateSetting>> settingApi,
            RootApi<ResponseApiEntities<ResultUploadFile>> fileDeleteApi)
        {
            _tokenService = tokenService;
            _settingApi = settingApi;
            _fileDeleteApi = fileDeleteApi;
        }

        public async Task DeleteFile(int ID, string endPoint)
        {
            List<ResultUploadFile> resultUploadImages = new List<ResultUploadFile>();
            List<ResultUploadFile> resultUploadFiles = new List<ResultUploadFile>();
            List<ResultUploadFile> resultUploadVideos = new List<ResultUploadFile>();

            // Use the injected _settingApi to get the entity
            var (resdataEdit, authorized) = await _settingApi.RunMethodApi($"{endPoint}/{ID}", null, method: Method.Get);

            if (resdataEdit.Entity?.JsonPicture != null)
            {
                resultUploadImages = JsonConvert.DeserializeObject<List<ResultUploadFile>>(resdataEdit.Entity.JsonPicture);
            }

            foreach (var model in resultUploadImages)
            {
                // Use the injected _fileDeleteApi to delete each file
                await _fileDeleteApi.RunMethodApi($"DeleteFile/{model.PathFileName}", null, method: Method.Delete);
            }

            // if (resdataEdit.Entity?.JsonFiles != null)
            //     resultUploadFiles = JsonConvert.DeserializeObject<List<ResultUploadFile>>(resdataEdit.Entity.JsonFiles);
            // foreach (var model in resultUploadFiles)
            // {
            //     await _fileDeleteApi.RunMethodApi($"DeleteFile/{model.PathFileName}", null, method: Method.Delete);
            // }

            // if (resdataEdit.Entity?.Jsonvideos != null)
            //     resultUploadVideos = JsonConvert.DeserializeObject<List<ResultUploadFile>>(resdataEdit.Entity.Jsonvideos);
            // foreach (var model in resultUploadVideos)
            // {
            //     await _fileDeleteApi.RunMethodApi($"DeleteFile/{model.PathFileName}", null, method: Method.Delete);
            // }
        }
    }
}