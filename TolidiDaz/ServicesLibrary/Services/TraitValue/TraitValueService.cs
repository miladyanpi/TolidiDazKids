using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.TraitValueSrv
{
    public sealed class TraitValueService : Repository<TraitValue>, ITraitValueService
    {
        public TraitValueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}