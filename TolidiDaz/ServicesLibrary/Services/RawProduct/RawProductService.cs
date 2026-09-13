using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.RawProductSrv
{
    public sealed class RawProductService : Repository<RawProduct>, IRawProductService
    {
        public RawProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}