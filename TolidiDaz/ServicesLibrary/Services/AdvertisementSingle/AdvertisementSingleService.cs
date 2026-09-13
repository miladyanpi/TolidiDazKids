using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.AdvertisementSingleSrv
{
    public sealed class AdvertisementSingleService : Repository<AdvertisementSingle>, IAdvertisementSingleService
    {
        public AdvertisementSingleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}