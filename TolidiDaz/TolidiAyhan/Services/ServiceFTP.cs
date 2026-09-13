
using Dto.Models.DtoUploadFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace TolidiAyhan.Models.Plugin
{
    public class ServiceFTP
    {
        public ServiceFTP()
        {
            HostName = "";
            UserName = "UserftpFiles";
            Password = "?sf80Kc5";
            pathRelative = "UserftpFiles@185.94.97.22/";
        }
        public string FtpRootPath { 
            get
            {
                UriBuilder UB = new UriBuilder
                {
                    Scheme = "FTP",
                    Host = HostName,
                    Path = $"{pathRelative}"
                };
                return UB.Uri.ToString();
            } 
            private set { }
        }    
        
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string pathRelative { get; set; }
        public  async Task<List<ResultUploadFile>> Upload(IList<IFormFile> fromFiles)
        {
            List<ResultUploadFile> resultFileNames = new List<ResultUploadFile>();

          
            foreach (var file in fromFiles)
            {
                var extention = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = DateTime.Now.Ticks + extention;
                UriBuilder UB = new UriBuilder
                {
                    Scheme = "FTP",
                    Host = HostName,
                    Path = $"{pathRelative}{fileName}"
                };
                var FTP = (FtpWebRequest)WebRequest.Create(requestUri: UB.Uri);
                FTP.KeepAlive = true;
                FTP.Timeout = Convert.ToInt32(new TimeSpan(hours: 0, minutes: 3, seconds: 0).TotalMilliseconds);
                FTP.Method = WebRequestMethods.Ftp.UploadFile;
                FTP.Credentials = new NetworkCredential(userName: UserName, password: Password);
                var filePath = Path.GetTempFileName();

                using (var FsInput = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(FsInput);
                }
                using (FileStream FsInput = new FileStream(path: filePath, mode: FileMode.Open, access: FileAccess.Read))
                {
                    try
                    {
                        var RS = await FTP.GetRequestStreamAsync();
                        //using (var RS = await FTP.GetRequestStreamAsync())
                        //{
                        byte[] Buffer = new byte[1024 * 1024];
                        int Read;
                        while ((Read = await FsInput.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
                        {
                            await RS.WriteAsync(Buffer, 0, Read);
                        }
                        await RS.FlushAsync();
                        RS.Close();
                        //}
                        FsInput.Close();
                    }
                   catch(Exception ex)
                    { 
                        Console.WriteLine(ex.ToString());
                    }
                }

                //using (Stream RS = await FTP.GetRequestStreamAsync())
                //{

                //    byte[] Buffer = new byte[1024 * 1024];
                //    int Read;
                //    while ((Read = await FsInput.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
                //    {
                //        await RS.WriteAsync(Buffer, 0, Read);
                //    }
                //    await RS.FlushAsync();
                //    RS.Close();
                //}

                //FsInput.Close();
                resultFileNames.Add(new ResultUploadFile
                {
                    PathFileName = fileName,
                });
            }
            //   FTP = null;
            return resultFileNames;
        }

        //public async Task Upload(string HostName, int Port, string UserName, string Password, string InputFilePath, string Folder)
        //{
        //    UriBuilder UB = new UriBuilder
        //    {
        //        Scheme = "FTP",
        //        Host = HostName,
        //        Port = Port,
        //        Path = $"{Folder}{Path.GetFileName(InputFilePath)}"
        //    };
        //    var FTP = (FtpWebRequest)WebRequest.Create(requestUri: UB.Uri);
        //    FTP.KeepAlive = true;
        //    FTP.Timeout = Convert.ToInt32(new TimeSpan(hours: 0, minutes: 3, seconds: 0).TotalMilliseconds);
        //    FTP.Method = WebRequestMethods.Ftp.UploadFile;
        //    FTP.Credentials = new NetworkCredential(userName: UserName, password: Password);
        //    using (FileStream FsInput = new FileStream(path: InputFilePath, mode: FileMode.Open, access: FileAccess.Read))
        //    {
        //        using (var RS = await FTP.GetRequestStreamAsync())
        //        {
        //            byte[] Buffer = new byte[1024 * 1024];
        //            int Read;
        //            while ((Read = await FsInput.ReadAsync(Buffer, 0, Buffer.Length)) > 0)
        //            {
        //                await RS.WriteAsync(Buffer, 0, Read);
        //            }
        //            await RS.FlushAsync();
        //            RS.Close();
        //        }
        //        FsInput.Close();
        //    }
        //    FTP = null;
        //}

        public async Task DeletFile(string FileName)
        {
            try
            {
                UriBuilder UB = new UriBuilder
                {
                    Scheme = "FTP",
                    Host = HostName,
                    Path = $"{pathRelative}{FileName}",
                };
                var FTP = (FtpWebRequest)WebRequest.Create(requestUri: UB.Uri);
                FTP.KeepAlive = true;
                FTP.Timeout = Convert.ToInt32(new TimeSpan(hours: 0, minutes: 3, seconds: 0).TotalMilliseconds);
                FTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FTP.Credentials = new NetworkCredential(userName: UserName, password: Password);
                
                var RS = (FtpWebResponse)await FTP.GetResponseAsync();
                RS.Close();
                FTP = null;
            }
            catch { }
        }
     
    }
}