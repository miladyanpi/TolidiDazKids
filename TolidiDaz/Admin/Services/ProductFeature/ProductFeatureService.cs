using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Admin.Services.Category;
using Dto.Models.Constant;
using Dto.Models.DtoBlogComment;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.ProductFeature
{
    public class ProductFeatureService:BaseService
    {

        private IRootApi<ResponseApiEntities<ResultProductFeature>> _RootApiResultProductFeature;
        public List<ResultProductFeature>? ResultProductFeatures { get; set; } = new List<ResultProductFeature>();
        private readonly CategoryService _CategoryService;
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public ProductFeatureService(
            CategoryService CategoryService,
        IRootApi<ResponseApiEntities<ResultProductFeature>> RootApiResultProductFeature)
        {
            _RootApiResultProductFeature = RootApiResultProductFeature;
            _CategoryService = CategoryService;

        }
       
        public async Task GetData(string? SearchText = "")
        {
            try
            {
                var resdata = await _RootApiResultProductFeature.RunMethodApi($"ProductFeatures/Data?CategoryID={_CategoryService.CategoryID}", SetPaging(SearchText), method: Method.Post);
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
    }
}
