using Admin.Services;
using Admin.Services.BaseShareService;
using Admin.Services.Category;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductFeature;
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
        IRootApi<ResponseApiEntities<UpdateProductFeatureValue>> RootApiUpdateProductFeatureValue,
        IRootApi<ResponseApiEntities<ResultProductFeature>> RootApiResultProductFeature,

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
        public List<AddProductFeatureValue> AddProductFeatureValues { get; set; } = new List<AddProductFeatureValue>();
        public List<UpdateProductFeatureValue> UpdateProductFeatureValues { get; set; } = new List<UpdateProductFeatureValue>();
        public List<ResultProductFeature> ResultProductFeatures { get; set; } = new List<ResultProductFeature>();

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
                    var resdata = await RootApiResultProduct.RunMethodApi($"Products/{ID}", null, method: Method.Delete);
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
                addProduct.CategoryID =CategoryID;
                if (addProduct.CategoryID == null)
                {
                     await Swal.FireAsync(new SweetAlertOptions
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
                     await Swal.FireAsync(new SweetAlertOptions
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
                     await Swal.FireAsync(new SweetAlertOptions
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
                     await Swal.FireAsync(new SweetAlertOptions
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
                    for (int i = 0; i < AddProductFeatureValues.Count; i++)
                    {
                        AddProductFeatureValues[i].ProductID = resdata.ID;
                    }
                    if (AddProductFeatureValues.Count > 0)
                    {
                        var resdataProductFeatureValues = await RootApiAddProductFeatureValue.RunMethodApi("ProductFeatureValues/List", AddProductFeatureValues, method: Method.Post);
                        AddProductFeatureValues.Clear();
                    }

                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addProduct = new AddProduct
                    {
                        Visible = true,
                        ProductExistStatus = EnumConstant.ProductExistStatus.Existent,
                    };
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
                //updateProduct.CategoryID = _CategoryService.CategoryID;
                if (updateProduct.CategoryID == null)
                {
                   await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "دسته بندی سطح 3 را انتخاب کنید",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;
                }
                if (updateProduct.Discount > updateProduct.Price)
                {
                     await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "تخفیف نمیتونه بیشتر از قیمت اصلی باشه",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;
                }
                if (updateProduct.Price == 0)
                {
                    await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = "قیمت اصلی نمیتونه صفر باشه",
                        Icon = ResultMessageApi.Error,
                        ShowConfirmButton = true,
                    });
                    return;

                }
                if (updateProduct.Count <= 0)
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
                var resdataEdit = await RootApiUpdateProduct.RunMethodApi("Products", updateProduct, method: Method.Patch);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    if (AddProductFeatureValues.Count > 0)
                    {
                        var resdataProductFeatureValues = await RootApiAddProductFeatureValue.RunMethodApi("ProductFeatureValues/List", AddProductFeatureValues, method: Method.Post);
                        AddProductFeatureValues.Clear();
                    }
                    if (UpdateProductFeatureValues.Count > 0)
                    {
                        var resdataProductFeatureValues = await RootApiUpdateProductFeatureValue.RunMethodApi("ProductFeatureValues/List", UpdateProductFeatureValues, method: Method.Patch);

                    }
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
                var resdataEdit = await RootApiUpdateProduct.RunMethodApi($"Products/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateProduct = resdataEdit.Entity;
                    var resdataProductFeatureValue = await RootApiUpdateProductFeatureValue.RunMethodApi($"ProductFeatureValues/All/{updateProduct.ID}", null, method: Method.Get);
                    if (resdataProductFeatureValue != null && resdataProductFeatureValue.Status == ResultMessageApi.Success)
                    {
                        UpdateProductFeatureValues = resdataProductFeatureValue.Entities.ToList();
                    }
                    var resdataProductFeature = await RootApiResultProductFeature.RunMethodApi($"ProductFeatures/All/{updateProduct.ResultCategory.ParentID}", null, method: Method.Get);
                    if (resdataProductFeature != null && resdataProductFeature.Status == ResultMessageApi.Success)
                    {
                        ResultProductFeatures = resdataProductFeature.Entities.ToList();
                    }
                    if (UpdateProductFeatureValues.Count == 0)
                    {
                        foreach (var item in ResultProductFeatures)
                        {
                            AddProductFeatureValues.Add(new AddProductFeatureValue
                            {
                                ResultProductFeature = item,
                                Value = null,
                                ProductID = updateProduct.ID,
                                ProductFeatureID = item.ID,
                            });
                        }
                    }
                    else if (UpdateProductFeatureValues.Count != ResultProductFeatures.Count)
                    {
                        foreach (var item in ResultProductFeatures)
                        {
                            if (UpdateProductFeatureValues.Where(s => s.ProductFeatureID == item.ID).FirstOrDefault() == null)
                            {
                                AddProductFeatureValues.Add(new AddProductFeatureValue
                                {
                                    ResultProductFeature = item,
                                    Value = null,
                                    ProductID = updateProduct.ID,
                                    ProductFeatureID = item.ID,
                                });
                            }

                        }
                    }
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
    }
}
