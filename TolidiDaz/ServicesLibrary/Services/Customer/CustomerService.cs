using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CustomerSrv
{
    public sealed class CustomerService:Repository<Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
