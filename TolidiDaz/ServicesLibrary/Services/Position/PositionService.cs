using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.PositionSrv
{
    public sealed class PositionService : Repository<Position>, IPositionService
    {
        public PositionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}