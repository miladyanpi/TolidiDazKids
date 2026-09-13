using Dto.Models.Constant;
using Dto.Models.DtoCategory;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
    public class CategoryService
    {
        private readonly IRootApi<ResponseApiEntities<ResultCategory>> _RootApiGet;
        public CategoryService(IRootApi<ResponseApiEntities<ResultCategory>> RootApiGet)
        {
            _RootApiGet = RootApiGet;
        }
        public List<ResultCategory>? ResultCategorys { get; set; } = new List<ResultCategory>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDate()
        {
            var resdata = await _RootApiGet.RunMethodApi($"Categorys/Public?ParentID={null}", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                ResultCategorys = resdata.Entities.ToList();

            }
            NotifyStateChanged();
        }
    }
}
