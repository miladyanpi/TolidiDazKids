using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.AboutSrv
{
    public sealed class AboutService : Repository<About>, IAboutService
    {

        public AboutService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}