using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CartSrv
{
    public sealed class CartService : Repository<Cart>, ICartService
    {
        public CartService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}