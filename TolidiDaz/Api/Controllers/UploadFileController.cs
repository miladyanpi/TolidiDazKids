using ServicesLibrary.Services.CustomerSrv;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


    public class UploadFileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerService _customerService;



        public UploadFileController(
            IConfiguration configuration,
            ICustomerService customerService

            )
        {
            _configuration = configuration;
            _customerService = customerService;
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)] // ۱۰۰ مگابایت
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Upload()
        {
            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();
            try
            {
                foreach(var file in  Request.Form.Files)
                {
                    var folderName = _configuration.GetSection("Storage:PathUploadFile").Value;
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (file.Length > 0)
                    {
                        var extention = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                        var fileName = DateTime.Now.Ticks + extention;
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            resultFileNames.Add(new ResultUploadFile
                            {
                                PathFileName = fileName,
                            });
                        }

                    }
                }
           
                return Ok(new ResponseApiEntities<ResultUploadFile>
                                                          (entities: resultFileNames,
                                                          statusCode: ResultMessageApi.SuccessCode,
                                                          status: ResultMessageApi.Success,
                                                          message: ResultMessageApi.UploadFilesSuccess,
                                                          countAllRecordTable: resultFileNames.Count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultUploadFile>
                                                   (entities: resultFileNames,
                                                   statusCode: ResultMessageApi.ErrorCode,
                                                   status: ResultMessageApi.Error,
                                                   message: ex.Message,
                                                   countAllRecordTable: resultFileNames.Count));
            }

        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[AllowAnonymous] // اگر نیاز به لاگین نیست، مراقب باش!
        //[HttpPost("Upload")]
        //public async Task<IActionResult> Upload([FromForm] IFormFile file) // مهم: فقط یک فایل یا List<IFormFile> files
        //{
        //    var resultFileNames = new List<ResultUploadFile>();

        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest(new ResponseApiEntities<ResultUploadFile>
        //                                         (entities: resultFileNames,
        //                                         statusCode: ResultMessageApi.ErrorCode,
        //                                         status: ResultMessageApi.Error,
        //                                         message: "فایلی انتخاب نشده است.",
        //                                         countAllRecordTable: resultFileNames.Count));
        //    }

        //    try
        //    {
        //        var folderName = _configuration.GetSection("Storage:PathUploadFile").Value;
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (!Directory.Exists(pathToSave))
        //            Directory.CreateDirectory(pathToSave);

        //        var extension = Path.GetExtension(file.FileName);
        //        var fileName = DateTime.Now.Ticks + extension;
        //        var fullPath = Path.Combine(pathToSave, fileName);
        //       // var dbPath = Path.Combine(folderName, fileName).Replace("\\", "/"); // برای ذخیره در دیتابیس

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        var resultFile = new ResultUploadFile
        //        {
        //            PathFileName = fileName,

        //        };

        //        resultFileNames.Add(resultFile);

        //        return Ok(new ResponseApiEntities<ResultUploadFile>
        //                                                  (entities: resultFileNames,
        //                                                  statusCode: ResultMessageApi.SuccessCode,
        //                                                  status: ResultMessageApi.Success,
        //                                                  message: ResultMessageApi.UploadFilesSuccess,
        //                                                  countAllRecordTable: resultFileNames.Count));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseApiEntities<ResultUploadFile>
        //                                     (entities: resultFileNames,
        //                                     statusCode: ResultMessageApi.ErrorCode,
        //                                     status: ResultMessageApi.Error,
        //                                     message: "خطا در ذخیره فایل: " + ex.Message,
        //                                     countAllRecordTable: resultFileNames.Count));
        //    }

        //}
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("DeleteFile/MultiFiles")]
        public async Task<IActionResult> DeleteMultiFiles([FromBody]  DeleteUploadFile deleteUploadFile)
        {
           
            try
            {
                foreach (var file in deleteUploadFile.ResultUploadFiles)
                {
                    var folderName = _configuration.GetSection("Storage:PathUploadFile").Value;
                    var pathForDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    var fullPath = Path.Combine(pathForDelete, file.PathFileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                       
                    }
                }
                return Ok(new ResponseApiEntities<ResultUploadFile>
                                                              (entities: deleteUploadFile.ResultUploadFiles,
                                                              statusCode: ResultMessageApi.SuccessCode,
                                                              status: ResultMessageApi.Success,
                                                              message: ResultMessageApi.DeleteFilesSuccess,
                                                              countAllRecordTable: deleteUploadFile.ResultUploadFiles.Count()));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultUploadFile>
                                                   (entities: deleteUploadFile.ResultUploadFiles,
                                                   statusCode: ResultMessageApi.SuccessCode,
                                                   status: ResultMessageApi.Success,
                                                   message: ex.Message,
                                                   countAllRecordTable: deleteUploadFile.ResultUploadFiles.Count));
            }

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("DeleteFile/{fileName}")]
        public async Task<IActionResult> Delete([FromRoute] string fileName)
        {
            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();
            try
            {
                var folderName = _configuration.GetSection("Storage:PathUploadFile").Value;
                var pathForDelete = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fullPath = Path.Combine(pathForDelete, fileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Ok(new ResponseApiEntities<ResultUploadFile>
                                                         (entities: resultFileNames,
                                                         statusCode: ResultMessageApi.SuccessCode,
                                                         status: ResultMessageApi.Success,
                                                         message: ResultMessageApi.DeleteFilesSuccess,
                                                         countAllRecordTable: 1));
                }

                return BadRequest(new ResponseApiEntities<ResultUploadFile>
                                                 (entities: resultFileNames,
                                                 statusCode: ResultMessageApi.ErrorCode,
                                                 status: ResultMessageApi.Error,
                                                 message: ResultMessageApi.DeleteFilesError,
                                                 countAllRecordTable: 0));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultUploadFile>
                                                   (entities: resultFileNames,
                                                   statusCode: ResultMessageApi.SuccessCode,
                                                   status: ResultMessageApi.Success,
                                                   message: ex.Message,
                                                   countAllRecordTable: resultFileNames.Count));
            }

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Ftp/DownloadFiles/Admin/OtherAllFiles")]
        public async Task<IActionResult> GetFtpFileAdminOtherAllFiles([FromQuery] string FileName)
        {
            // FTP اطلاعات
            var folderName = _configuration.GetSection("public_html:PublicFolderUploadFiles").Value;
            var hostName = _configuration["public_html:HostName"];
            var userName = _configuration["public_html:UserName"];
            var password = _configuration["public_html:Password"];
            int port = int.Parse(_configuration.GetSection("public_html:port").Value);

            var pathRelative = _configuration["public_html:pathRelative"];

            var targetFtpDirectoryPath = $"{pathRelative}/{folderName}";

            UriBuilder ub = new UriBuilder
            {
                Scheme = "FTP",
                Host = hostName,
                Port = port,
                Path = $"{targetFtpDirectoryPath}/{FileName}"
            };

            var ftp = (FtpWebRequest)WebRequest.Create(ub.Uri);
            ftp.Method = WebRequestMethods.Ftp.DownloadFile;
            ftp.Credentials = new NetworkCredential(userName, password);
            ResultDownloadFile resultDownloadFile = new ResultDownloadFile();
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)await ftp.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await responseStream.CopyToAsync(ms);
                            resultDownloadFile.Content = ms.ToArray();
                        }
                    }
                }
                var extention = "." + FileName.Split('.')[FileName.Split('.').Length - 1];

                resultDownloadFile.FileExtention = extention;
                return Ok(new ResponseApiEntity<ResultDownloadFile>
                                             (entity: resultDownloadFile,
                                             statusCode: ResultMessageApi.SuccessCode,
                                             status: ResultMessageApi.Success,
                                             message: ResultMessageApi.DownloadFilesError));

            }
            catch
            {
                return BadRequest(new ResponseApiEntity<ResultDownloadFile>
                                              (entity: new ResultDownloadFile(),
                                              statusCode: ResultMessageApi.ErrorCode,
                                              status: ResultMessageApi.Error,
                                              message: ResultMessageApi.DownloadFilesErrorNotPermission));
            }
        }
        [HttpPost("Ftp/UploadFiles/OtherAllFiles")]
        public async Task<IActionResult> UploadFtp()
        {

            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();
            try
            {
                var file = Request.Form.Files[0];
                var folderName = _configuration.GetSection("public_html:PublicFolderUploadFiles").Value;
                var HostName = _configuration.GetSection("public_html:HostName").Value;
                var UserName = _configuration.GetSection("public_html:UserName").Value;
                var Password = _configuration.GetSection("public_html:Password").Value;
                int port = int.Parse(_configuration.GetSection("public_html:port").Value);
                var pathRelative = _configuration.GetSection("public_html:pathRelative").Value;
                var targetFtpDirectoryPath1 = $"{pathRelative}/{folderName}";

                _ = await CreateFtpDirectory(HostName, UserName, Password, port, targetFtpDirectoryPath1);


                if (file.Length > 0)
                {
                    var extention = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    var fileName = DateTime.Now.Ticks + extention;
                    UriBuilder UB = new UriBuilder
                    {
                        Scheme = "FTP",
                        Host = HostName,
                        Port = port,
                        Path = $"{targetFtpDirectoryPath1}/{fileName}"
                    };
                    var FTP = (FtpWebRequest)WebRequest.Create(requestUri: UB.Uri);
                    FTP.KeepAlive = true;
                    FTP.Timeout = Convert.ToInt32(new TimeSpan(hours: 0, minutes: 3, seconds: 0).TotalMilliseconds);
                    FTP.Method = WebRequestMethods.Ftp.UploadFile;
                    FTP.Credentials = new NetworkCredential(userName: UserName, password: Password);

                    var filePath = Path.GetTempFileName();
                    try
                    {
                        using (var FsInput = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(FsInput);
                            resultFileNames.Add(new ResultUploadFile
                            {
                                PathFileName = fileName,

                            });
                        }
                        using (FileStream FsInput = new FileStream(path: filePath, mode: FileMode.Open, access: FileAccess.Read))
                        {
                            using (var RS = await FTP.GetRequestStreamAsync())
                            {
                                byte[] Buffer = new byte[4096];
                                int Read;
                                while ((Read = await FsInput.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
                                {
                                    await RS.WriteAsync(Buffer, 0, Read);
                                }
                                await RS.FlushAsync();
                            }

                            using (FtpWebResponse response = (FtpWebResponse)await FTP.GetResponseAsync())
                            {
                                response.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                return Ok(new ResponseApiEntities<ResultUploadFile>
                                                          (entities: resultFileNames,
                                                          statusCode: ResultMessageApi.SuccessCode,
                                                          status: ResultMessageApi.Success,
                                                          message: ResultMessageApi.UploadFilesSuccess,
                                                          countAllRecordTable: resultFileNames.Count));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultUploadFile>
                                                   (entities: resultFileNames,
                                                   statusCode: ResultMessageApi.ErrorCode,
                                                   status: ResultMessageApi.Error,
                                                   message: ResultMessageApi.UploadFilesError + "-" + ex.Message,
                                                   countAllRecordTable: resultFileNames.Count));
            }

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Ftp/DeleteFile/MultiFiles/OtherAllFiles")]
        public async Task<IActionResult> DeleteFtpFileOtherAllFiles([FromBody] DeleteUploadFile deleteUploadFile)
        {
            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();

            try
            {
                // گرفتن اطلاعات FTP
                var folderName = _configuration.GetSection("public_html:PublicFolderUploadFiles").Value;
                var hostName = _configuration["public_html:HostName"];
                var userName = _configuration["public_html:UserName"];
                var password = _configuration["public_html:Password"];
                int port = int.Parse(_configuration.GetSection("public_html:port").Value);

                var pathRelative = _configuration["public_html:pathRelative"];

                var targetFtpDirectoryPath = $"{pathRelative}/{folderName}";
                foreach (var item in deleteUploadFile.ResultUploadFiles)
                {
                    UriBuilder ub = new UriBuilder
                    {
                        Scheme = "FTP",
                        Host = hostName,
                        Port = port,
                        Path = $"{targetFtpDirectoryPath}/{item.PathFileName}"
                    };
                    var ftp = (FtpWebRequest)WebRequest.Create(ub.Uri);
                    ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                    ftp.Credentials = new NetworkCredential(userName, password);

                    using (var response = (FtpWebResponse)await ftp.GetResponseAsync())
                    {
                        response.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<ResultUploadFile>(
                    entity: new ResultUploadFile(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.DeleteFilesErrorNotFound));
            }


            return Ok(new ResponseApiEntity<ResultUploadFile>(
                    entity: new ResultUploadFile(),
                    statusCode: ResultMessageApi.SuccessCode,
                    status: ResultMessageApi.Success,
                    message: ResultMessageApi.DeleteFilesSuccess));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Ftp/DeleteFile/OtherAllFiles")]
        public async Task<IActionResult> DeleteFtpFileOtherAllFiles([FromQuery]  string FileName)
        {
            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();

            try
            {
                // گرفتن اطلاعات FTP
                var folderName = _configuration.GetSection("public_html:PublicFolderUploadFiles").Value;
                var hostName = _configuration["public_html:HostName"];
                var userName = _configuration["public_html:UserName"];
                var password = _configuration["public_html:Password"];
                int port = int.Parse(_configuration.GetSection("public_html:port").Value);

                var pathRelative = _configuration["public_html:pathRelative"];
                var targetFtpDirectoryPath = $"{pathRelative}/{folderName}";

                UriBuilder ub = new UriBuilder
                {
                    Scheme = "FTP",
                    Host = hostName,
                    Port = port,
                    Path = $"{targetFtpDirectoryPath}/{FileName}"
                };
                var ftp = (FtpWebRequest)WebRequest.Create(ub.Uri);
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                ftp.Credentials = new NetworkCredential(userName, password);

                using (var response = (FtpWebResponse)await ftp.GetResponseAsync())
                {
                    response.Close();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<ResultUploadFile>(
                    entity: new ResultUploadFile(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.DeleteFilesErrorNotFound));
            }


            return Ok(new ResponseApiEntity<ResultUploadFile>(
                    entity: new ResultUploadFile(),
                    statusCode: ResultMessageApi.SuccessCode,
                    status: ResultMessageApi.Success,
                    message: ResultMessageApi.DeleteFilesSuccess));
        }
        /// <summary>
        /// یک دایرکتوری جدید را در سرور FTP ایجاد می‌کند.
        /// </summary>
        private async Task<bool> CreateFtpDirectory(string hostName, string userName, string password, int port, string directoryPath)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "ftp",
                Host = hostName,
                Port = port,
                Path = directoryPath
            };

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uriBuilder.Uri);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(userName, password);
                request.UsePassive = true;   // خیلی مهم برای بعضی سرورها
                request.UseBinary = true;
                request.KeepAlive = false;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    Console.WriteLine($"پوشه FTP ایجاد شد: {response.StatusDescription}");
                    response.Close();
                    return true;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                // کد 550 در اینجا می‌تواند به این معنی باشد که پوشه از قبل وجود دارد (اگر سرور خطای مشخص‌تری ندهد).
                // یا اینکه کاربر مجوز لازم برای ایجاد پوشه را ندارد.
                if (response != null && response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.WriteLine($"پوشه FTP از قبل وجود دارد یا خطای دسترسی: {response.StatusDescription}");
                    response.Close();
                    // در برخی سرورها، MakeDirectory اگر پوشه وجود داشته باشد، خطای 550 می‌دهد.
                    // بنابراین، اگر بعد از بررسی اولیه این خطا را گرفتیم، ممکن است پوشه از قبل وجود داشته باشد.
                    // اما برای اطمینان بیشتر، می‌توانید یک بار دیگر FtpDirectoryExistsAndCreate را فراخوانی کنید.
                    // برای سادگی، فعلاً فرض می‌کنیم اگر 550 گرفتیم، و قبلاً بررسی کرده‌ایم، یعنی از قبل وجود دارد.
                    return true;
                }
                Console.WriteLine($"خطا هنگام ایجاد پوشه FTP: {ex.Message}");
                if (response != null) response.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطای غیرمنتظره هنگام ایجاد پوشه FTP: {ex.Message}");
                return false;
            }
        }
    }
}
