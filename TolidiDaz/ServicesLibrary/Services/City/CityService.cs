using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CitySrv
{
    public sealed class CityService : Repository<City>, ICityService
    {
        public CityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}