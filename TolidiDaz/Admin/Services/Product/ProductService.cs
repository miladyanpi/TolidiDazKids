using Admin.Services;
using Admin.Services.BaseShareService;
using Admin.Services.Category;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductFeatureValue;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;
using System.ComponentModel;
using static Dto.Enum.EnumConstant;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Admin.Services.Product
{

    public class ProductService(
        IRootApi<ResponseApiEntities<ResultProduct>> _RootApiResultProducts,
        IRootApi<ResponseApiEntity<ResultProduct>> RootApiResultProduct,
        IRootApi<ResponseApiEntity<AddProduct>> RootApiAddProduct,
        IRootApi<ResponseApiEntity<UpdateProduct>> RootApiUpdateProduct,
        IRootApi<ResponseApiEntities<AddProductFeatureValue>> RootApiAddProductFeatureValue,

        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public bool SearchAllFlag  { get; set; } = false;
        [DisplayName("دسته بندی سطح 3")]
        public int? CategoryID { get; set; }

        public UpdateProduct? updateProduct { get; set; } = new();
        public AddProduct? addProduct { get; set; } = new()
        {
            Visible = true,
            ProductExistStatus= ProductExistStatus.Existent,
        };
        public ResultProduct resultProduct { get; set; } = new();
        public List<ResultProduct>? ResultProducts= new List<ResultProduct>();

        public string? guid { get; set; }
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                ResponseApiEntities<ResultProduct> resdata;
                if (SearchAllFlag)
                {
                     resdata = await _RootApiResultProducts.RunMethodApi($"Products/Data", SetPaging(SearchText), method: Method.Post);

                }
                else
                {
                     resdata = await _RootApiResultProducts.RunMethodApi($"Products/Data?CategoryID={CategoryID}", SetPaging(SearchText), method: Method.Post);

                }
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultProducts = resdata.Entities.ToList() ?? [];
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
                    var resdata = await RootApiResultProduct.RunMethodApi($"Products/{ID}", null, method: Method.Delete);
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
                addProduct.CategoryID =CategoryID;
                if (addProduct.CategoryID == null)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "دسته بندی سطح 3 را انتخاب کنید",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;
                }
                if (addProduct.Discount > addProduct.Price)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "تخفیف نمیتونه بیشتر از قیمت اصلی باشه",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;
                }
                if (addProduct.Price == 0)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "قیمت اصلی نمیتونه صفر باشه",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;

                }
                if (addProduct.Count <= 0)
                {
                    var result2 = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "تعداد نمیتونه صفر باشه",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;

                }
                var resdata = await RootApiAddProduct.RunMethodApi("Products", addProduct, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    //for (int i = 0; i < AddProductFeatureValues.Count; i++)
                    //{
                    //    AddProductFeatureValues[i].ProductID = resdata.ID;
                    //}
                    //if (AddProductFeatureValues.Count > 0)
                    //{
                    //    var resdataProductFeatureValues = await RootApiAddProductFeatureValue.RunMethodApi("ProductFeatureValues/List", AddProductFeatureValues, method: Method.Post);
                    //    AddProductFeatureValues.Clear();
                    //}

                    //await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addProduct = new AddProduct
                    {
                        Visible = true,
                        ProductExistStatus = EnumConstant.ProductExistStatus.Existent,
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
                var resdataEdit = await RootApiUpdateProduct.RunMethodApi("Products", updateProduct, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateProduct.RunMethodApi($"Products/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateProduct = resdataEdit.Entity;
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
