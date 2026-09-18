using Admin.Services.BaseShareService;
using Admin.Services.MageManagementService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoTeam;
using Dto.Models.DtoSlider;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RestSharp;
using static Dto.Enum.EnumConstant;

namespace Admin.Services.Team
{

    public class TeamService(
        IRootApi<ResponseApiEntities<ResultTeam>> _RootApiResultTeams,
        IRootApi<ResponseApiEntity<ResultTeam>> RootApiResultTeam,
        IRootApi<ResponseApiEntity<AddTeam>> RootApiAddTeam,
        IRootApi<ResponseApiEntity<UpdateTeam>> RootApiUpdateTeam,
        IRootApi<ResponseApiEntity<UpdateJsonFile>> RootApiUpdateJsonFile,
        IRootApi<ResponseApiEntities<ResultUploadFile>> RootApiResultUploadFile,
        IJSRuntime JS,
        SweetAlertService Swal
        ) : BaseService, IImageManagementByFtpService
    {
        public UpdateTeam? updateTeam { get; set; } = new();
        public AddTeam? addTeam { get; set; } = new();
        public ResultTeam? resultTeam = new ResultTeam();
        public List<ResultTeam>? ResultTeams = new List<ResultTeam>();
        public List<ResultUploadFile> ResultUploadImages { get;  set; } = [];
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultTeams.RunMethodApi($"Teams/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultTeams = resdata.Entities.ToList() ?? [];
                    SetPerPage(resdata.CountAllRecordTable);
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
        public async Task GetAllDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultTeams.RunMethodApi($"Teams/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultTeams = resdata.Entities.ToList() ?? [];
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
                    var resdata = await RootApiResultTeam.RunMethodApi($"Teams/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddTeam.RunMethodApi("Teams", addTeam, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addTeam = new();
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
                var resdataEdit = await RootApiUpdateTeam.RunMethodApi("Teams", updateTeam, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateTeam.RunMethodApi($"Teams/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateTeam = resdataEdit.Entity;
                    if (updateTeam.JsonPicture != null)
                        ResultUploadImages = JsonConvert.DeserializeObject<List<ResultUploadFile>>(updateTeam.JsonPicture);
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
                        ID = updateTeam.ID,
                        JsonPicture = JsonConvert.SerializeObject(ResultUploadImages),
                        EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image
                    };
                    _ = await RootApiUpdateJsonFile.RunMethodApi("Teams/UpdateJsonFile", updateJsonFile, method: Method.Patch);
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
                    ID = updateTeam.ID,
                    JsonPicture = JsonConvert.SerializeObject(resultUploadImages),
                    EnumJsonImageFileVideo = EnumJsonImageFileVideo.Image
                };
                _ = await RootApiUpdateJsonFile.RunMethodApi("Teams/UpdateJsonFile", updateJsonFile, method: Method.Patch);
                NotifyStateChanged();
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
}
