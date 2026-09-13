using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.OrderSrv
{
    public sealed class OrderService : Repository<Order>, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}