using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ServicesLibrary.Services.StorageSrv
{
    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;

        public StorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Upload(List<IFormFile> fromFile)
        {
            int count = 0;
            var pathRelative = _configuration.GetSection("Storage:PathUploadFile").Value;
            string fileName;
            try
            {
                foreach (var file in fromFile)
                {
                    var extention = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = DateTime.Now.Ticks + extention;
                    var pathBuild = Path.Combine(Directory.GetCurrentDirectory(), pathRelative);
                    if (!Directory.Exists(pathBuild))
                    {
                        Directory.CreateDirectory(pathBuild);
                    }
                    var path = Path.Combine(pathRelative, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        count++;
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }
    }
}
