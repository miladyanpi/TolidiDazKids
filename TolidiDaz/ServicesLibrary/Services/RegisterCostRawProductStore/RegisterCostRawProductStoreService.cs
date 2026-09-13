using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.RegisterCostRawProductStoreSrv
{
    public sealed class RegisterCostRawProductStoreService : Repository<RegisterCostRawProductStore>, IRegisterCostRawProductStoreService
    {
        public RegisterCostRawProductStoreService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}