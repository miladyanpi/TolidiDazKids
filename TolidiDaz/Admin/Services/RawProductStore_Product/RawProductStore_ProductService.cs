using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Dto.Models.DtoRawProductStore_Product;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.RawProductStore_Product
{
    public class RawProductStore_ProductService(
        IRootApi<ResponseApiEntities<ResultRawProductStore_Product>> _RootApiRawProductStore_Product) : BaseService
    {

        public List<ResultRawProductStore_Product>? ResultRawProductStore_Products { get; set; } = new List<ResultRawProductStore_Product>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task GetData(int? RawProductStoreID)
        {
            var resdata = await _RootApiRawProductStore_Product.RunMethodApi($"RawProductStore_Products/ByRawProductStoreID?RawProductStoreID={RawProductStoreID}", null, method: Method.Get);
            if (resdata != null)
            {
                ResultRawProductStore_Products = resdata.Entities.ToList();
            }
            else
            {
                ResultRawProductStore_Products = new();
            }
            NotifyStateChanged();

        }

    }
}
