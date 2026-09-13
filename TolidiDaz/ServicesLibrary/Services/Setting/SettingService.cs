using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.SettingSrv
{
    public sealed class SettingService : Repository<Setting>, ISettingService
    {
        public SettingService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
