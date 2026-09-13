using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.AdvertisementSrv
{
    public sealed class AdvertisementService : Repository<Advertisement>, IAdvertisementService
    {
        public AdvertisementService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}