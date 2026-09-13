using Dto.Models.DtoCartItem;

namespace TolidiAyhan.Services.Cart
{
    public class CartCalculatorPriceService
    {
        public event Action? OnChange;
        public List<ResultCartItem> Items { get; private set; } = new();
        public bool Loaded { get; private set; } = false;

        public long SumPrice => Items.Sum(x => x.ResultProduct.Price * x.Quantity);
        public long SumDiscount => Items.Sum(x => x.ResultProduct.Discount * x.Quantity);
        public long SumFinal => SumPrice - SumDiscount;

        public void SetItems(List<ResultCartItem> items)
        {
            Items = items;
            Loaded = true;
            Notify();
        }
        private void Notify() => OnChange?.Invoke();
    }
}
