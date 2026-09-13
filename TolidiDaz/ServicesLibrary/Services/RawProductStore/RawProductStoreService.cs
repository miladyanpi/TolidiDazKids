using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.RawProductStoreSrv
{
    public sealed class RawProductStoreService : Repository<RawProductStore>, IRawProductStoreService
    {
        public RawProductStoreService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}