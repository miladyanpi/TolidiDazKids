using Admin.Services.BaseShareService;
using Admin.Services.MageManagementService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoStory;
using Dto.Models.DtoLable;
using Dto.Models.DtoSlider;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestSharp;
using static Dto.Enum.EnumConstant;

namespace Admin.Services.Story
{
    public class StoryService(
        IRootApi<ResponseApiEntities<ResultStory>> _RootApiResultStorys,
        IRootApi<ResponseApiEntity<ResultStory>> RootApiResultStory,
        IRootApi<ResponseApiEntity<AddStory>> RootApiAddStory,
        IRootApi<ResponseApiEntity<UpdateStory>> RootApiUpdateStory,
        IRootApi<ResponseApiEntity<UpdateJsonFile>> RootApiUpdateJsonFile,
        IRootApi<ResponseApiEntities<ResultUploadFile>> RootApiResultUploadFile,
        IJSRuntime JS,
        SweetAlertService Swal
        ) : BaseService, IImageManagementByFtpService
    {
        private List<AddJsonLable> AddJsonLables { get; set; } = new List<AddJsonLable>();

        public UpdateStory? updateStory { get; set; } = new();
        public AddStory? addStory { get; set; } = new();
        public List<ResultStory>? ResultStorys = new List<ResultStory>();
        public List<ResultUploadFile> ResultUploadImages { get; set; } = [];
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultStorys.RunMethodApi($"Storys/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultStorys = resdata.Entities.ToList() ?? [];
                    SetPerPage(resdata.CountAllRecordTable);
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
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
                    var resdata = await RootApiResultStory.RunMethodApi($"Storys/{ID}", null, method: Method.Delete);
                    if (resdata != null && resdata.Status == ResultMessageApi.Success)
                    {
                        await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                        await GetDataAsync();
                    }
                    else if (resdata != null && resdata.Status == ResultMessageApi.Error)
                    {
                         await Swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "پیام",
                            Text = resdata.Message,
                            Icon = resdata.Status,
                            ShowConfirmButton = true,
                        });
                    }
                    else
                    {
                         await Swal.FireAsync(new SweetAlertOptions
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
                    await Swal.FireAsync(new SweetAlertOptions
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
                var resdata = await RootApiAddStory.RunMethodApi("Storys", addStory, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addStory = new();
                }
                else if (resdata != null && resdata.Status == ResultMessageApi.Error)
                {
                     await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = resdata.Message,
                        Icon = resdata.Status,
                        ShowConfirmButton = true,
                    });
                }
                else
                {
                     await Swal.FireAsync(new SweetAlertOptions
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
                await Swal.FireAsync(new SweetAlertOptions
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
                var resdataEdit = await RootApiUpdateStory.RunMethodApi("Storys", updateStory, method: Method.Patch);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdataEdit.Message, resdataEdit.Status);
                }
                else if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Error)
                {
                     await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = resdataEdit.Message,
                        Icon = resdataEdit.Status,
                        ShowConfirmButton = true,
                    });
                }
                else
                {
                     await Swal.FireAsync(new SweetAlertOptions
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
                await Swal.FireAsync(new SweetAlertOptions
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
                var resdataEdit = await RootApiUpdateStory.RunMethodApi($"Storys/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateStory = resdataEdit.Entity;
                    if (updateStory.JsonPicture != null)
                        ResultUploadImages = JsonConvert.DeserializeObject<List<ResultUploadFile>>(updateStory.JsonPicture);
                    else
                        ResultUploadImages = new List<ResultUploadFile>();
                        NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                await Swal.FireAsync(new SweetAlertOptions
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
                        ID = updateStory.ID,
                        JsonPicture = JsonConvert.SerializeObject(ResultUploadImages),
                        EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image

                    };
                    _ = await RootApiUpdateJsonFile.RunMethodApi("Storys/UpdateJsonFile", updateJsonFile, method: Method.Patch);
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                await Swal.FireAsync(new SweetAlertOptions
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
                    ID = updateStory.ID,
                    JsonPicture = JsonConvert.SerializeObject(resultUploadImages),
                    EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image
                };
                _ = await RootApiUpdateJsonFile.RunMethodApi("Storys/UpdateJsonFile", updateJsonFile, method: Method.Patch);
            }
            catch (Exception ex)
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }
        public void SetDataLable(List<AddJsonLable> addJsonLables)
        {
            AddJsonLables = addJsonLables;
        }
    }
}
