using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CustomerSrv
{
    public interface ICustomerService:IRepository<Customer>
    {
    }
}
