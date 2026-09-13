using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.OrderItemSrv
{
    public sealed class OrderItemService : Repository<OrderItem>, IOrderItemService
    {
        public OrderItemService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}