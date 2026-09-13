using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.RefreshTokenEntitySrv
{
    public sealed class RefreshTokenEntityService : Repository<RefreshTokenEntity>, IRefreshTokenEntityService
    {
        public RefreshTokenEntityService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}