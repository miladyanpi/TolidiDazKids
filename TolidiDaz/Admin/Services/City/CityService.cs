using Admin.Components.Pages.Component;
using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCity;
using Dto.Models.DtoProvince;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminPanel.Services.City
{

    public class CityService(
        IRootApi<ResponseApiEntities<ResultCity>> _RootApiResultCitys,
        IRootApi<ResponseApiEntity<ResultCity>> RootApiResultCity,
        IRootApi<ResponseApiEntity<AddCity>> RootApiAddCity,
        IRootApi<ResponseApiEntity<UpdateCity>> RootApiUpdateCity,
        IRootApi<ResponseApiEntities<ResultProvince>> RootApiResultProvinces,
        IRootApi<ResponseApiEntities<ResultCity>> RootApiGet,
        IRootApi<ResponseApiEntity<ResultCity>> RootApiDelete,
        IRootApi<ResponseApiEntities<ResultUploadFile>> RootApiResultUploadFiles,
        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public UpdateCity? updateCity { get; set; } = new();
        public AddCity? addCity { get; set; } = new();
        public List<ResultCity>? ResultCitys= new List<ResultCity>();
        public string? guid { get; set; }
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultCitys.RunMethodApi($"Citys/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultCitys = resdata.Entities.ToList() ?? [];
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
                    var resdata = await RootApiResultCity.RunMethodApi($"Citys/{ID}", null, method: Method.Delete);
                    if (resdata != null && resdata.Status == ResultMessageApi.Success)
                    {
                        await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                        await GetDataAsync();
                    }
                    else if (resdata != null && resdata.Status == ResultMessageApi.Error)
                    {
                        _ = await Swal.FireAsync(new SweetAlertOptions
                        {
                            Title = "پیام",
                            Text = resdata.Message,
                            Icon = resdata.Status,
                            ShowConfirmButton = true,
                        });
                    }
                    else
                    {
                        _ = await Swal.FireAsync(new SweetAlertOptions
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
                }
            }
        }
        public async Task AddAsync()
        {
            try
            {
                var resdata = await RootApiAddCity.RunMethodApi("Citys", addCity, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addCity = new();
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
            }
        }
        public async Task UpdateAsync()
        {
            try
            {
                var resdataEdit = await RootApiUpdateCity.RunMethodApi("Citys", updateCity, method: Method.Patch);
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
            }
        }
        public async Task GetUpdateDataAsync()
        {
            try
            {
                var resdataEdit = await RootApiUpdateCity.RunMethodApi($"Citys/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateCity = resdataEdit.Entity;
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
