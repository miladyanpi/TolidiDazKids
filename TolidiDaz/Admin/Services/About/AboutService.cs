using Admin.Services.BaseShareService;
using Admin.Services.MageManagementService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoAbout;
using Dto.Models.DtoSlider;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestSharp;
using static Dto.Enum.EnumConstant;

namespace Admin.Services.About
{

    public class AboutService(
        IRootApi<ResponseApiEntities<ResultAbout>> _RootApiResultAbouts,
        IRootApi<ResponseApiEntity<ResultAbout>> RootApiResultAbout,
        IRootApi<ResponseApiEntity<AddAbout>> RootApiAddAbout,
        IRootApi<ResponseApiEntity<UpdateAbout>> RootApiUpdateAbout,
        IRootApi<ResponseApiEntity<UpdateJsonFile>> RootApiUpdateJsonFile,
        IRootApi<ResponseApiEntities<ResultUploadFile>> RootApiResultUploadFile,
        IJSRuntime JS,
        SweetAlertService Swal
        ) : BaseService, IImageManagementByFtpService
    {
        public bool IsUpdateStatuse { get; set; } = false;
        public UpdateAbout? updateAbout { get; set; } = new();
        public AddAbout? addAbout { get; set; } = new();
        public ResultAbout? ResultAbout = new ResultAbout();
        public List<ResultUploadFile> ResultUploadImages { get;  set; } = [];
        
        public async Task<string?> GetLastDataAsync()
        {
            string guid = string.Empty;
            try
            {
                var resdata = await RootApiUpdateAbout.RunMethodApi($"Abouts/LastRecord", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    updateAbout = resdata.Entity;
                     guid = resdata.Entity.IdentityCode.ToString();
                    IsUpdateStatuse = true;
                }
                else
                {
                    IsUpdateStatuse = false;
                }
            }
            catch (Exception ex)
            {
                IsUpdateStatuse = false;
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
            NotifyStateChanged();
            return guid;
        }
        public async Task DeleteAsync(int ID)
        {
            var result1 = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "برای حذف مطمئن هستید؟",
                Text = "درصورت تایید شما، رکورد انتخاب شده حذف خواهد شد",
                ShowCancelButton = true,
                ConfirmButtonText = "بله",
                CancelButtonText = "خیر"
            });
            if (!string.IsNullOrEmpty(result1.Value))
            {
                try
                {
                    var resdata = await RootApiResultAbout.RunMethodApi($"Abouts/{ID}", null, method: Method.Delete);
                    if (resdata != null && resdata.Status == ResultMessageApi.Success)
                    {
                        await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                        await GetLastDataAsync();
                    }
                    else if (resdata != null && resdata.Status == ResultMessageApi.Error)
                    {
                        var result2 = await Swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "پیام",
                            Text = resdata.Message,
                            Icon = resdata.Status,
                            ShowConfirmButton = true,
                        });
                    }
                    else
                    {
                        var result2 = await Swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "پیام",
                            Text = ResultMessageApi.ErrorDisconnectApi,
                            Icon = ResultMessageApi.Error,
                            ShowConfirmButton = true,
                        });
                    }
                }
                catch (Exception ex)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = ex.ToString(),
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                }
            }
        }
        public async Task AddAsync()
        {
            try
            {
                var resdata = await RootApiAddAbout.RunMethodApi("Abouts", addAbout, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addAbout = new();
                }
                else if (resdata != null && resdata.Status == ResultMessageApi.Error)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = resdata.Message,
                        Icon = resdata.Status,
                        ShowConfirmButton = true,
                    });
                }
                else
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = ResultMessageApi.ErrorDisconnectApi,
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                }
            }
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        public async Task UpdateAsync()
        {
            try
            {
                var resdataEdit = await RootApiUpdateAbout.RunMethodApi("Abouts", updateAbout, method: Method.Patch);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdataEdit.Message, resdataEdit.Status);
                }
                else if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Error)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = resdataEdit.Message,
                        Icon = resdataEdit.Status,
                        ShowConfirmButton = true,
                    });
                }
                else
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = ResultMessageApi.ErrorDisconnectApi,
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                }
            }
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        public async Task GetUpdateDataAsync(string guid)
        {
            try
            {
                var resdataEdit = await RootApiUpdateAbout.RunMethodApi($"Abouts/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateAbout = resdataEdit.Entity;
                    if (updateAbout.JsonPicture != null)
                        ResultUploadImages = JsonConvert.DeserializeObject<List<ResultUploadFile>>(updateAbout.JsonPicture);
                    else
                        ResultUploadImages = new List<ResultUploadFile>();
                        NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        public async Task AddToListImagesAsync(List<ResultUploadFile> resultUploadFiles)
        {
            try
            {
                ResultUploadImages.AddRange(resultUploadFiles);
                if (ResultUploadImages != null)
                {
                    UpdateJsonFile updateJsonFile = new UpdateJsonFile
                    {
                        ID = updateAbout.ID,
                        JsonPicture = JsonConvert.SerializeObject(ResultUploadImages),
                        EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image
                    };
                    _ = await RootApiUpdateJsonFile.RunMethodApi("Abouts/UpdateJsonFile", updateJsonFile, method: Method.Patch);
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        public async Task DeleteImage(List<ResultUploadFile> resultUploadImages)
        {
            try
            {
                UpdateJsonFile updateJsonFile = new UpdateJsonFile
                {
                    ID = updateAbout.ID,
                    JsonPicture = JsonConvert.SerializeObject(resultUploadImages),
                    EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image

                };
                _ = await RootApiUpdateJsonFile.RunMethodApi("Abouts/UpdateJsonFile", updateJsonFile, method: Method.Patch);
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        
    }
}
