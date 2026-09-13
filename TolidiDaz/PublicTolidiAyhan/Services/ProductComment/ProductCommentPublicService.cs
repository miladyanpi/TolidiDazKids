using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Models.Constant;
using Dto.Models.DtoProductComment;
using Dto.Models.ResponseApi;
using RestSharp;
namespace PublicTolidiAyhan.Services.ProductComment
{
    public class ProductCommentPublicService(
        IRootApi<ResponseApiEntity<AddProductComment>> _RootApiAddProductComment,
        IRootApi<ResponseApiEntities<ResultProductComment>> _RootApiResultProductComments,
        SweetAlertService Swal)
    {
       public 
            (int RateCount1,
            int RateCount2,
            int RateCount3,
            int RateCount4,
            int RateCount5,
            double RateCountPercent1,
            double RateCountPercent2,
            double RateCountPercent3,
            double RateCountPercent4,
            double RateCountPercent5,
            int TotalRating) RatingCounts = new();

        public int? ProductID { get; set; }
        public AddProductComment? addProductComment { get; set; } = new ();
        public List<ResultProductComment?> resultProductComments { get; set; } = new ();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDataAsync()
        {
            addProductComment.ProductID = ProductID;
            var resdata = await _RootApiResultProductComments.RunMethodApi($"ProductComments/Public/All?ProductID={ProductID}", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                resultProductComments = resdata.Entities.ToList();
                RatingCounts.RateCount1 = resultProductComments.Where(s => s.Rating == 1.0).Count();
                RatingCounts.RateCount2 = resultProductComments.Where(s => s.Rating == 2.0).Count();
                RatingCounts.RateCount3 = resultProductComments.Where(s => s.Rating == 3.0).Count();
                RatingCounts.RateCount4 = resultProductComments.Where(s => s.Rating == 4.0).Count();
                RatingCounts.RateCount5 = resultProductComments.Where(s => s.Rating == 5.0).Count();
                RatingCounts.TotalRating =
                   RatingCounts.RateCount1 +
                   RatingCounts.RateCount2 +
                   RatingCounts.RateCount3 +
                   RatingCounts.RateCount4 +
                   RatingCounts.RateCount5;
                if (RatingCounts.TotalRating > 0)
                {
                    RatingCounts.RateCountPercent1 = Math.Round((double)RatingCounts.RateCount1 / (double)RatingCounts.TotalRating * 100, 2);
                    RatingCounts.RateCountPercent2 = Math.Round((double)RatingCounts.RateCount2 / (double)RatingCounts.TotalRating * 100, 2);
                    RatingCounts.RateCountPercent3 = Math.Round((double)RatingCounts.RateCount3 / (double)RatingCounts.TotalRating * 100, 2);
                    RatingCounts.RateCountPercent4 = Math.Round((double)RatingCounts.RateCount4 / (double)RatingCounts.TotalRating * 100, 2);
                    RatingCounts.RateCountPercent5 = Math.Round((double)RatingCounts.RateCount5 / (double)RatingCounts.TotalRating * 100, 2);

                }
                else
                {
                    RatingCounts.RateCountPercent1 = 0;
                    RatingCounts.RateCountPercent2 = 0;
                    RatingCounts.RateCountPercent3 = 0;
                    RatingCounts.RateCountPercent4 = 0;
                    RatingCounts.RateCountPercent5 = 0;

                }

                NotifyStateChanged();
            }
        }
        public async Task AddAsync()
        {
            addProductComment.ProductID = ProductID;
            var resdata = await _RootApiAddProductComment.RunMethodApi($"ProductComments?Key={Dto.Enum.Keys.CustomerSatisfactionKey}", addProductComment, method: Method.Post);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                addProductComment = new AddProductComment();
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
