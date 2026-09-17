using Admin.Services;
using Admin.Services.BaseShareService;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.DtoPaginagion;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoGroupQuestion;
using Dto.Models.DtoQuestion;
using Dto.Models.ResponseApi;
using Microsoft.JSInterop;
using RestSharp;

namespace Admin.Services.Question
{

    public class QuestionService(
        IRootApi<ResponseApiEntities<ResultGroupQuestion>> _RootApiResultGroupQuestions,
        IRootApi<ResponseApiEntities<ResultQuestion>> _RootApiResultQuestions,
        IRootApi<ResponseApiEntity<ResultQuestion>> RootApiResultQuestion,
        IRootApi<ResponseApiEntity<AddQuestion>> RootApiAddQuestion,
        IRootApi<ResponseApiEntity<UpdateQuestion>> RootApiUpdateQuestion,
        IJSRuntime JS,
        SweetAlertService Swal
        ) :BaseService
    {
        public UpdateQuestion? updateQuestion { get; set; } = new();
        public AddQuestion? addQuestion { get; set; } = new();
        public ResultQuestion resultQuestion { get; set; } = new();
        public List<ResultQuestion>? ResultQuestions= new List<ResultQuestion>();
        public List<ResultGroupQuestion>? ResultGroupQuestions = new List<ResultGroupQuestion>();
        public string? guid { get; set; }
        public async Task GetDataAsync(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultQuestions.RunMethodApi($"Questions/Data", SetPaging(SearchText), method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultQuestions = resdata.Entities.ToList() ?? [];
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
                var resdata = await _RootApiResultQuestions.RunMethodApi($"Questions/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultQuestions = resdata.Entities.ToList();
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
                    var resdata = await RootApiResultQuestion.RunMethodApi($"Questions/{ID}", null, method: Method.Delete);
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
                var resdata = await RootApiAddQuestion.RunMethodApi("Questions", addQuestion, method: Method.Post);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    await JS.InvokeVoidAsync(ToastConstant.FunctionJavasScriptName, resdata.Message, resdata.Status);
                    addQuestion = new();
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
                var resdataEdit = await RootApiUpdateQuestion.RunMethodApi("Questions", updateQuestion, method: Method.Patch);
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
                var resdataEdit = await RootApiUpdateQuestion.RunMethodApi($"Questions/ByGuid/{guid}", null, method: Method.Get);
                if (resdataEdit != null && resdataEdit.Status == ResultMessageApi.Success)
                {
                    updateQuestion = resdataEdit.Entity;
                    NotifyStateChanged();
                }
                var resdata = await _RootApiResultGroupQuestions.RunMethodApi($"GroupQuestions/All", null, method: Method.Get);
                if (resdata != null && resdata.Status == ResultMessageApi.Success)
                {
                    ResultGroupQuestions = resdata.Entities.ToList();
                    if (updateQuestion.GroupQuestionID == null)
                        updateQuestion.GroupQuestionID = ResultGroupQuestions.Count > 0 ? ResultGroupQuestions[0].ID : null;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
