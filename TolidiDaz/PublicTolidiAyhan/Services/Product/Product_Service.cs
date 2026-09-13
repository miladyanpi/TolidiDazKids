using Dto.Models.DtoFavoritUserProduct;
using Dto.Models.DtoProduct;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
   
    public class Product_Service(
      IRootApi<ResponseApiEntities<ResultProduct>> _RootApiGet)
    {

        public List<ResultProduct>? ResultProducts { get; set; } = new List<ResultProduct>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListData()
        {
            var resdata = await _RootApiGet.RunMethodApi($"Products/NewProductTop50", null, method: Method.Get);
            if (resdata != null)
            {
                ResultProducts = resdata.Entities.ToList();
            }
            else
            {
                ResultProducts = new();
            }
            NotifyStateChanged();

        }
       
    }
}
