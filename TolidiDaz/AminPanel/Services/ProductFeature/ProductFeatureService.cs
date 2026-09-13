using AdminPanel.Base;
using AdminPanel.Services;
using AminPanel.Pages.Component;
using AminPanel.Services.BaseShareService;
using AminPanel.Services.Category;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using RestSharp;

namespace AminPanel.Services.ProductFeature
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
        public async Task GetData(string SearchText = "")
        {
            var Search = Base.GetSearchText(SearchText);
            var resdata = await _RootApiResultProductFeature.RunMethodApi($"ProductFeatures?Page={pagingModel.Page}&Take={pagingModel.Take}{SearchText}&CategoryID={_CategoryService.CategoryID}", null, method: Method.Get);
            Count = resdata.CountAllRecordTable;
            ResultProductFeatures = resdata.Entities.ToList();
            pagingModel.CountPage = Math.Ceiling(Count / pagingModel.Take);
            pagingModel.CountPagePer10 = Math.Ceiling(pagingModel.CountPage / 10);
            NotifyStateChanged();

        }

    }
}
