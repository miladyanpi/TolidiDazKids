using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CustomerAddressSrv
{
    public sealed class CustomerAddressService : Repository<CustomerAddress>, ICustomerAddressService
    {
        public CustomerAddressService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}