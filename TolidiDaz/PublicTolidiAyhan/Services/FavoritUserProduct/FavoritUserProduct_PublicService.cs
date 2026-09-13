using Dto.Models.DtoFavoritUserProduct;
using Dto.Models.ResponseApi;
using RestSharp;
namespace PublicTolidiAyhan.Services
{
    public class FavoritUserProduct_PublicService(
        IRootApi<ResponseApiEntities<ResultFavoritUserProduct>> _RootApiFavoritUserProduct) 
    {

        public List<ResultFavoritUserProduct>? ResultFavoritUserProducts { get; set; } = new List<ResultFavoritUserProduct>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task GetData()
        {
            var resdata = await _RootApiFavoritUserProduct.RunMethodApi($"FavoritUserProducts/AllDataForCurrentUser", null, method: Method.Get);
            if (resdata != null)
            {
                ResultFavoritUserProducts = resdata.Entities.ToList();
            }
            else
            {
                ResultFavoritUserProducts = new();
            }
            NotifyStateChanged();

        }
        public bool CheckExistsProduct(int? ProductID)
        {
           var q= ResultFavoritUserProducts.Where(s => s.ProductID == ProductID).FirstOrDefault();
            if (q != null)
                return true;
            return false;

        }
    }
}
