using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoCategoryTrait;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;

namespace Admin.Services.CategoryTrait
{

    public class CategoryTraitService(
        IRootApi<ResponseApiEntities<ResultCategoryTrait>> _RootApiResultCategoryTraits,
        IRootApi<ResponseApiEntity<ResultCategoryTrait>> RootApiResultCategoryTrait,
        IRootApi<ResponseApiEntity<AddCategoryTrait>> RootApiAddCategoryTrait,
        IRootApi<ResponseApiEntity<UpdateCategoryTrait>> RootApiUpdateCategoryTrait,
        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public UpdateCategoryTrait? updateCategoryTrait { get; set; } = new();
        public AddCategoryTrait? addCategoryTrait { get; set; } = new();
        public ResultCategoryTrait resultCategoryTrait { get; set; } = new();
        public List<ResultCategoryTrait>? ResultCategoryTraits= new List<ResultCategoryTrait>();
        public List<AddCategoryTraitSelect>? AddCategoryTraitSelects= new List<AddCategoryTraitSelect>();
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultCategoryTraits.RunMethodApi($"CategoryTraits/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultCategoryTraits = resdata.Entities.ToList() ?? [];
                    SetPerPage(resdata.CountAllRecordTable);
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
        public async Task GetAllDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultCategoryTraits.RunMethodApi($"CategoryTraits/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultCategoryTraits = resdata.Entities.ToList();
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
                    var resdata = await RootApiResultCategoryTrait.RunMethodApi($"CategoryTraits/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddCategoryTrait.RunMethodApi("CategoryTraits", addCategoryTrait, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addCategoryTrait = new();
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
                var resdataEdit = await RootApiUpdateCategoryTrait.RunMethodApi("CategoryTraits", updateCategoryTrait, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateCategoryTrait.RunMethodApi($"CategoryTraits/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateCategoryTrait = resdataEdit.Entity;
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

    }

}
