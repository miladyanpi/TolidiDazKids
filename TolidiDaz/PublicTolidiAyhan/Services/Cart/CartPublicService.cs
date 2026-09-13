using Dto.Models.Constant;
using Dto.Models.DtoCartItem;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Components.Authorization;
using RestSharp;


namespace PublicTolidiAyhan.Services
{
    public class CartPublicService(
        IRootApi<ResponseApiEntity<AddCartItem>> _RootApiAddCartItem,
            CartCalculatorPriceService _cartCalculatorPriceService,
            IRootApi<ResponseApiEntities<ResultCartItem>> _RootApiResultCartItems
        )
    {
        public int Count { get; private set; } = 0;

        public List<ResultCartItem> resultCartItems { get; set; } = new List<ResultCartItem>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task<(string? Status, string? Message)> AddToCart(int productID)
        {

            AddCartItem addCartItem = new AddCartItem
            {
                ProductID = productID,
                Quantity = 1,
            };
            var resdata = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public", addCartItem, method: Method.Post);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                Count++;
                await GetCartItems();
                NotifyStateChanged();
                return (ResultMessageApi.Success, resdata.Message);

            }
            else if (resdata != null && resdata.Status == ResultMessageApi.Error)
            {
                return (ResultMessageApi.Error, resdata.Message);

            }
            else
                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");



        }
        public async Task<(string? Status, string? Message)> AddToCart1Item(int productID)
        {

            AddCartItem addCartItem = new AddCartItem
            {
                ProductID = productID,
                Quantity = 1,
            };
            var resdata = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public/Add", addCartItem, method: Method.Post);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                Count++;
                NotifyStateChanged();
                await GetCartItems();
                return (ResultMessageApi.Success, resdata.Message);

            }
            else if (resdata != null && resdata.Status == ResultMessageApi.Error)
            {
                return (ResultMessageApi.Error, resdata.Message);

            }
            else
                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");




        }
        public async Task<(string Status, string Message)> SubFromCart1Item(int productID)
        {

            AddCartItem addCartItem = new AddCartItem
            {
                ProductID = productID,
                Quantity = 1,
            };
            var resdata = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public/Sub", addCartItem, method: Method.Post);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                Count--;
                NotifyStateChanged();
                await GetCartItems();
                return (ResultMessageApi.Success, resdata.Message);

            }
            else
                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");



        }

        public async Task GetCartItems()
        {
            var resdata = await _RootApiResultCartItems.RunMethodApi($"CartItems/Public", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                _cartCalculatorPriceService.SetItems(resdata.Entities.ToList());
                Count = resdata.Entities.Sum(s => s.Quantity);
                resultCartItems = resdata.Entities.ToList();
                // await _cartCalculatorPriceService.GetSumFinal();
                NotifyStateChanged();

            }
        }
    }
}
