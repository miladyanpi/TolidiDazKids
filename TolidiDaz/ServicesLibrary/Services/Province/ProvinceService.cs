using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProvinceSrv
{
    public sealed class ProvinceService : Repository<Province>, IProvinceService
    {
        public ProvinceService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}