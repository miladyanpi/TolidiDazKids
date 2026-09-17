using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoBlog;
using Dto.Models.DtoBlogComment;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;

namespace Admin.Services.BlogComment
{

    public class BlogCommentService(
        IRootApi<ResponseApiEntities<ResultBlogComment>> _RootApiResultBlogComments,
        IRootApi<ResponseApiEntity<ResultBlogComment>> RootApiResultBlogComment,
        IRootApi<ResponseApiEntity<AddBlogComment>> RootApiAddBlogComment,
        IRootApi<ResponseApiEntity<UpdateBlogComment>> RootApiUpdateBlogComment,
        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public UpdateBlogComment? updateBlogComment { get; set; } = new();
        public AddBlogComment? addBlogComment { get; set; } = new();
        public List<ResultBlogComment>? ResultBlogComments= new List<ResultBlogComment>();
        public ResultBlogComment resultBlogComment = new ResultBlogComment();

        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultBlogComments.RunMethodApi($"BlogComments/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultBlogComments = resdata.Entities.ToList() ?? [];
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
                var resdata = await _RootApiResultBlogComments.RunMethodApi($"BlogComments/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultBlogComments = resdata.Entities.ToList();
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
                    var resdata = await RootApiResultBlogComment.RunMethodApi($"BlogComments/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddBlogComment.RunMethodApi("BlogComments", addBlogComment, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addBlogComment = new();
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
                var resdataEdit = await RootApiUpdateBlogComment.RunMethodApi("BlogComments", updateBlogComment, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateBlogComment.RunMethodApi($"BlogComments/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateBlogComment = resdataEdit.Entity;
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
        public async Task SetStatusMessage(int ID, EnumConstant.CommentStatus commentStatus)
        {
            var result1 = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "پیام ارسالی کاربر مورد تایید است؟",
                Text = "درصورت تایید شما، پیام کاربر دربخش بلاگ های صفحه عمومی نمایش داده خواهد شد",
                ShowCancelButton = true,
                ConfirmButtonText = "بله",
                CancelButtonText = "خیر"
            });
            if (!string.IsNullOrEmpty(result1.Value))
            {
                var resdata = await _RootApiResultBlogComments.RunMethodApi($"BlogComments/SetStatus?Id={ID}&commentStatus={commentStatus}", null, method: Method.Get);
                if (resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    await GetDataAsync();
                }
                else
                {
                    _ = await Swal.FireAsync(new SweetAlertOptions
                    {
                        Title = "پیام",
                        Text = resdata.Message,
                        Icon = resdata.Status,
                        ShowConfirmButton = true,
                    });
                }
            }
        }
    }
}
