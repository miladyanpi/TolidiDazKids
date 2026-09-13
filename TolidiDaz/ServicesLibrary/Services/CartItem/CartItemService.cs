using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CartItemSrv
{
    public sealed class CartItemService : Repository<CartItem>, ICartItemService
    {
        public CartItemService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}