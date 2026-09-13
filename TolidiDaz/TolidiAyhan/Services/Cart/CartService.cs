
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Models.Constant;
using Dto.Models.DtoCartItem;
using Dto.Models.DtoProduct;
using Dto.Models.DtpPayment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using RestSharp;
using System.Xml.Linq;
using TolidiAyhan.Services.TolidiAyhan.Services;
using static Dto.Enum.EnumConstant;

namespace TolidiAyhan.Services.Cart
{
    public class CartService 
    {
        public int Count { get; private set; } = 0;

        private bool IsLoggedIn;
        private readonly AuthenticationStateProvider _AuthStateProvider;
        private readonly RootApi<ResponseApiEntity<AddCartItem>> _RootApiAddCartItem;
        private readonly RootApi<ResponseApiEntities<ResultCartItem>> _RootApiResultCartItems;
        private readonly CartCalculatorPriceService _cartCalculatorPriceService;
        private readonly TokenServiceServer _tokenServiceServer;
        public List<ResultCartItem> resultCartItems { get; set; } = new List<ResultCartItem>();
        public event Action? OnChange=null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public CartService(
            RootApi<ResponseApiEntity<AddCartItem>> RootApiAddCartItem,
            AuthenticationStateProvider AuthStateProvider,
            CartCalculatorPriceService cartCalculatorPriceService,
            TokenServiceServer tokenServiceServer,
            RootApi<ResponseApiEntities<ResultCartItem>> RootApiResultCartItems)
        {
            _RootApiAddCartItem = RootApiAddCartItem;
            _AuthStateProvider = AuthStateProvider;
            _RootApiResultCartItems = RootApiResultCartItems;
            _cartCalculatorPriceService = cartCalculatorPriceService;
            _tokenServiceServer= tokenServiceServer;
        }
        public async Task<(string? Status, string? Message)> AddToCart(int productID)
        {
            var authState = await _AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            IsLoggedIn = user.Identity?.IsAuthenticated ?? false;
            if (IsLoggedIn)
            {
                AddCartItem addCartItem = new AddCartItem
                {
                    ProductID = productID,
                    Quantity = 1,
                };
                var (resdata, authorized) = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public", addCartItem, method: Method.Post);
                if (authorized)
                {
                    Count++;
                    NotifyStateChanged();
                    return (ResultMessageApi.Success, resdata.Message);

                }
                else
                    return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");

            }
            else
            {

                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");
            }

        }
        public async Task<(string? Status, string? Message)> AddToCart1Item(int productID)
        {
            var authState = await _AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            IsLoggedIn = user.Identity?.IsAuthenticated ?? false;
            if (IsLoggedIn)
            {
                AddCartItem addCartItem = new AddCartItem
                {
                    ProductID = productID,
                    Quantity = 1,
                };
                var (resdata, authorized) = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public/Add", addCartItem, method: Method.Post);
                if (authorized)
                {
                    Count++;
                    NotifyStateChanged();
                    return (ResultMessageApi.Success, resdata.Message);

                }
                else
                    return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");


            }
            else
            {
                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");
            }

        }
        public async Task<(string Status, string Message)> SubFromCart1Item(int productID)
        {
            var authState = await _AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            IsLoggedIn = user.Identity?.IsAuthenticated ?? false;
            if (IsLoggedIn)
            {
                AddCartItem addCartItem = new AddCartItem
                {
                    ProductID = productID,
                    Quantity = 1,
                };
                var (resdata, authorized) = await _RootApiAddCartItem.RunMethodApi($"CartItems/Public/Sub", addCartItem, method: Method.Post);
                if (authorized)
                {
                    Count--;
                    NotifyStateChanged();
                    return (ResultMessageApi.Success, resdata.Message);

                }
                else
                    return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");

            }
            else
            {
                return (ResultMessageApi.Error, "ابتدا وارد اکانت خود شوید");
            }

        }
      
        public async  Task<bool> GetCartItems()
        {
           

            var authState = await _AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            IsLoggedIn = user.Identity?.IsAuthenticated ?? false;
            if (IsLoggedIn)
            {
                var (resdata, authorized) = await _RootApiResultCartItems.RunMethodApi($"CartItems/Public", null, method: Method.Get);
                if (authorized)
                {
                    _cartCalculatorPriceService.SetItems(resdata.Entities.ToList());
                    Count = resdata.Entities.Sum(s => s.Quantity);
                    resultCartItems = resdata.Entities.ToList();
                    NotifyStateChanged();
                    return true;
                }
                else
                {
                 
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }
    }
}
