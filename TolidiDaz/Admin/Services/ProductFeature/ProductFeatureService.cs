using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductFeature;
using Dto.Models.DtoProductFeatureValue;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;
using System.ComponentModel;

namespace Admin.Services.ProductFeature
{

    public class ProductFeatureService(
        IRootApi<ResponseApiEntities<ResultProductFeature>> _RootApiResultProductFeatures,
        IRootApi<ResponseApiEntity<ResultProductFeature>> RootApiResultProductFeature,
        IRootApi<ResponseApiEntity<AddProductFeature>> RootApiAddProductFeature,
        IRootApi<ResponseApiEntity<UpdateProductFeature>> RootApiUpdateProductFeature,
        IJSRuntime JS,
        SweetAlertService Swal
        ) : BaseService
    {
        [DisplayName("دسته بندی سطح 2")]
        public int? CategoryID { get; set; }
        public UpdateProductFeature? updateProductFeature { get; set; } = new();
        public AddProductFeature? addProductFeature { get; set; } = new() { 
        Visible= true,
        };
        public ResultProductFeature resultProductFeature { get; set; } = new();
        public List<ResultProductFeature>? ResultProductFeatures = new List<ResultProductFeature>();
        public List<AddProductFeatureValue> AddProductFeatureValues { get; set; } = new List<AddProductFeatureValue>();

        public string? guid { get; set; }
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultProductFeatures.RunMethodApi($"ProductFeatures/Data?CategoryID={CategoryID}", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultProductFeatures = resdata.Entities.ToList() ?? [];
                    SetPerPage(resdata.CountAllRecordTable);
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public async Task GetAllDataAsync()
        {
            try
            {
                AddProductFeatureValues.Clear();
                var resdata = await _RootApiResultProductFeatures.RunMethodApi($"ProductFeatures/All/{CategoryID}", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultProductFeatures = resdata.Entities.ToList() ?? [];
                    foreach (var item in ResultProductFeatures)
                    {
                        AddProductFeatureValues.Add(new AddProductFeatureValue
                        {
                            ResultProductFeature = item,
                            Value = null,
                            ProductFeatureID = item.ID,
                        });
                    }
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
                    var resdata = await RootApiResultProductFeature.RunMethodApi($"ProductFeatures/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddProductFeature.RunMethodApi("ProductFeatures", addProductFeature, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addProductFeature = new()
                    {
                        Visible = true,
                    };
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
                var resdataEdit = await RootApiUpdateProductFeature.RunMethodApi("ProductFeatures", updateProductFeature, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateProductFeature.RunMethodApi($"ProductFeatures/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateProductFeature = resdataEdit.Entity;
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
