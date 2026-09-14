using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoGroupBlog;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;

namespace Admin.Services.GroupBlog
{

    public class GroupBlogService(
        IRootApi<ResponseApiEntities<ResultGroupBlog>> _RootApiResultGroupBlogs,
        IRootApi<ResponseApiEntity<ResultGroupBlog>> RootApiResultGroupBlog,
        IRootApi<ResponseApiEntity<AddGroupBlog>> RootApiAddGroupBlog,
        IRootApi<ResponseApiEntity<UpdateGroupBlog>> RootApiUpdateGroupBlog,
        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public UpdateGroupBlog? updateGroupBlog { get; set; } = new();
        public AddGroupBlog? addGroupBlog { get; set; } = new();
        public ResultGroupBlog resultGroupBlog { get; set; } = new();
        public List<ResultGroupBlog>? ResultGroupBlogs= new List<ResultGroupBlog>();
        public string? guid { get; set; }
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultGroupBlogs.RunMethodApi($"GroupBlogs/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultGroupBlogs = resdata.Entities.ToList() ?? [];
                    SetPerPage(resdata.CountAllRecordTable);
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public async Task GetAllDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultGroupBlogs.RunMethodApi($"GroupBlogs/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultGroupBlogs = resdata.Entities.ToList();
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
                    var resdata = await RootApiResultGroupBlog.RunMethodApi($"GroupBlogs/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddGroupBlog.RunMethodApi("GroupBlogs", addGroupBlog, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addGroupBlog = new();
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
                var resdataEdit = await RootApiUpdateGroupBlog.RunMethodApi("GroupBlogs", updateGroupBlog, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateGroupBlog.RunMethodApi($"GroupBlogs/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateGroupBlog = resdataEdit.Entity;
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
