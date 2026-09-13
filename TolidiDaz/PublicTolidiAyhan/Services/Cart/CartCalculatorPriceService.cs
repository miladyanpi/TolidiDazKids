using Dto.Models.DtoCartItem;
using ServicesLibrary.Services.PricingRuleSrv;
using System.Threading.Tasks;

namespace PublicTolidiAyhan.Services
{
    public class CartCalculatorPriceService(IPricingRuleService _pricingRuleService, LoginStatusCheckService _LoginStatusCheckService)
    {
        public event Action? OnChange;
        public List<ResultCartItem> Items { get; private set; } = new();
        public bool Loaded { get; private set; } = false;
        public long SumFinal=0;
        public long SumDiscount = 0;
        public long SumPrice = 0;
        public async Task<long> GetSumPrice()
        {
            Int64 Sum = 0;
            var ResulLoginStatus = await _LoginStatusCheckService.GetLoginStatusCheck();
            List<string> Roles=new List<string>();
            Roles.Add(ResulLoginStatus.Role);
            foreach (var item in Items) 
            {
                var Price = _pricingRuleService.CalculatePrice(item.ResultProduct, item.Quantity, Roles, ResulLoginStatus.IsLoggedIn);
                Sum += Price * item.Quantity;
            }
            SumPrice= Sum;
            return SumPrice; //Items.Sum(x => x.ResultProduct.Price * x.Quantity);
        }
        public long GetSumDiscount()
        {
            SumDiscount= Items.Sum(x => x.ResultProduct.Discount * x.Quantity);
            return SumDiscount;
        }
        public async Task GetSumFinal()
        {
            SumFinal= await GetSumPrice() - GetSumDiscount();
        }

        public async Task SetItems(List<ResultCartItem> items)
        {
            Items = items;
            Loaded = true;
            await GetSumFinal();
            Notify();

        }
        private void Notify() => OnChange?.Invoke();
    }
}
