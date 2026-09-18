using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoCategory;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;

namespace Admin.Services.Category
{

    public class CategoryService(
        IRootApi<ResponseApiEntities<ResultCategory>> _RootApiResultCategorys,
        IRootApi<ResponseApiEntity<ResultCategory>> RootApiResultCategory,
        IRootApi<ResponseApiEntity<AddCategory>> RootApiAddCategory,
        IRootApi<ResponseApiEntity<UpdateCategory>> RootApiUpdateCategory,
        IJSRuntime JS,
        SweetAlertService Swal
        ) : BaseService
    {
        public UpdateCategory? updateCategory { get; set; } = new();
        public AddCategory? addCategory { get; set; } = new() { 
        Visible=true,
        
        };
        public ResultCategory resultCategory { get; set; } = new();

        public List<ResultCategory>? ResultCategorys = new List<ResultCategory>();
        public List<ResultCategory>? ResultCategorys1 = new List<ResultCategory>();
        public List<ResultCategory>? ResultCategorys2 { get; set; } = new List<ResultCategory>();
        public List<ResultCategory>? ResultCategorys3 { get; set; } = new List<ResultCategory>();
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultCategorys.RunMethodApi($"Categorys/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultCategorys = resdata.Entities.ToList() ?? [];
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
        public async Task GetAllDataAsync()
        {
            try
            {
                var resdata = await _RootApiResultCategorys.RunMethodApi($"Categorys/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultCategorys1 = resdata.Entities.ToList();
                }
                else
                {
                    ResultCategorys1 = new();
                }
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
        public async Task GetAllDataAsync2()
        {
            try
            {
                var resdata = await _RootApiResultCategorys.RunMethodApi("Categorys/All", null, method: Method.Get);

                if (resdata?.Status == ResultMessageApi.Success && resdata.Entities is not null)
                {
                    // ۱. دریافت دسته‌های سطح اول
                    ResultCategorys1 = resdata.Entities.ToList();

                    // ۲. فلت کردن سریع و ایمن دسته‌های سطح دوم با LINQ و جلوگیری از Null
                    ResultCategorys2 = ResultCategorys1
                        .Where(c => c.ResultCategorys is not null)
                        .SelectMany(c => c.ResultCategorys)
                        .ToList();
                }
                else
                {
                    // ریست کردن هر دو لیست در صورت عدم موفقیت API
                    ResultCategorys1 = new();
                    ResultCategorys2 = new();
                }
            }
            catch (Exception ex)
            {
                // در صورت بروز خطا هم لیست‌ها ایمن خالی می‌مانند
                ResultCategorys1 = new();
                ResultCategorys2 = new();

                // TODO: اگر ILogger داری ex رو اینجا لاگ کن
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "خطا در دریافت اطلاعات",
                    Text = "متأسفانه در برقراری ارتباط با سرور خطایی رخ داده است.",
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                    ConfirmButtonText = "متوجه شدم"
                });
            }
            finally
            {
                // با خیال راحت وضعیت کامپوننت را در هر حالتی به‌روزرسانی می‌کنیم
                NotifyStateChanged();
            }
        }

        public void GetCategoryParent2InAdd()
        {
            var q = ResultCategorys1.Where(s => s.ID == addCategory.ParentID1).FirstOrDefault();
            if (q != null)
            {
                ResultCategorys2 = q.ResultCategorys;

            }
            else
            {
                ResultCategorys2 = new();
            }
            NotifyStateChanged();

        }
        public async Task GetCategoryParent2InAdd2()
        {
            ResultCategorys2.Clear();
            if (addCategory.ParentID1==0|| addCategory.ParentID1==null)
            {
                await GetAllDataAsync2();
            }
            else
            {
                var q = ResultCategorys1.Where(s => s.ID == addCategory.ParentID1).FirstOrDefault();
                if (q != null)
                {
                    ResultCategorys2 = q.ResultCategorys;
                

                }
                else
                {
                    ResultCategorys2 = new();
                }
            }
            NotifyStateChanged();
        }
        public void GetCategoryParent3InAdd()
        {
            var q = ResultCategorys2.Where(s => s.ID == addCategory.ParentID2).FirstOrDefault();
            if (q != null)
            {
                ResultCategorys3 = q.ResultCategorys;

            }
            else
            {
                ResultCategorys3 = new();
            }
            NotifyStateChanged();

        }
        public void GetCategoryParent2InUpdate()
        {
            var q = ResultCategorys1.Where(s => s.ID == updateCategory.ParentID1).FirstOrDefault();
            if (q != null)
            {
                ResultCategorys2 = q.ResultCategorys;
                updateCategory.ParentID2 = updateCategory.ParentResultCategory != null && updateCategory.ParentResultCategory.ParentID > 0 ? updateCategory.ParentResultCategory.ID : 0;

            }
            else
            {
                ResultCategorys2 = new();
                updateCategory.ParentID2 = null;
            }
            NotifyStateChanged();

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
                    var resdata = await RootApiResultCategory.RunMethodApi($"Categorys/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddCategory.RunMethodApi("Categorys", addCategory, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addCategory = new() { Visible=true};
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
                if (updateCategory.ParentID1 > 0 && updateCategory.ParentID2 > 0)
                    updateCategory.ParentID = updateCategory.ParentID2;
                else
                    updateCategory.ParentID = updateCategory.ParentID1;
                var resdataEdit = await RootApiUpdateCategory.RunMethodApi("Categorys", updateCategory, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateCategory.RunMethodApi($"Categorys/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateCategory = resdataEdit.Entity;
                    updateCategory.ParentID1 = updateCategory.ParentResultCategory != null && updateCategory.ParentResultCategory.ParentID > 0 ? updateCategory.ParentResultCategory.ParentID : updateCategory.ParentResultCategory.ID;
                    GetCategoryParent2InUpdate();
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
