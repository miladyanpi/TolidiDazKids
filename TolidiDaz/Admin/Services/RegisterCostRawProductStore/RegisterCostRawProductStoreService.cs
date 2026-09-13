using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Dto.Models.DtoRawProductStore_Product;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.RawProductStore_Product
{
    public class RegisterCostRawProductStoreService(
        IRootApi<ResponseApiEntities<ResultRawProductStore_Product>> _RootApiRawProductStore_Product) : BaseService
    {

        public List<ResultRawProductStore_Product>? ResultRawProductStore_Products { get; set; } = new List<ResultRawProductStore_Product>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task Calculate(int? RawProductStoreID)
        {
          
            NotifyStateChanged();

        }

    }
}
