using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.OrderPaymentTempSrv
{
    public sealed class OrderPaymentTempService : Repository<OrderPaymentTemp>, IOrderPaymentTempService
    {
        public OrderPaymentTempService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}