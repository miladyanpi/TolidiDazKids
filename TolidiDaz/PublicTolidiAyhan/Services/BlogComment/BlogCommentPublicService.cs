using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Models.Constant;
using Dto.Models.DtoBlogComment;
using Dto.Models.ResponseApi;
using RestSharp;
namespace PublicTolidiAyhan.Services.BlogComment
{
    public class BlogCommentPublicService(
        IRootApi<ResponseApiEntity<AddBlogComment>> _RootApiAddBlogComment,
        IRootApi<ResponseApiEntities<ResultBlogComment>> _RootApiResultBlogComments,
        SweetAlertService Swal)
    {
        public int? BlogID { get; set; }
        public AddBlogComment? addBlogComment { get; set; } = new ();
        public List<ResultBlogComment?> resultBlogComments { get; set; } = new ();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDataAsync()
        {
            addBlogComment.BlogID = BlogID;
            var resdata = await _RootApiResultBlogComments.RunMethodApi($"BlogComments/Public/All?BlogID={BlogID}", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                resultBlogComments = resdata.Entities.ToList();
                NotifyStateChanged();
            }
        }
        public async Task AddAsync()
        {
            addBlogComment.BlogID = BlogID;
            var resdata = await _RootApiAddBlogComment.RunMethodApi($"BlogComments?Key={Dto.Enum.Keys.CustomerSatisfactionKey}", addBlogComment, method: Method.Post);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                addBlogComment = new AddBlogComment();
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = "نظر شما با موفقیت ارسال شد",
                    Icon = resdata.Status,
                    ShowConfirmButton = true,
                });
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
        }
        
       
    }
}
