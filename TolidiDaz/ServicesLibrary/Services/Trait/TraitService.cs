using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.TraitSrv
{
    public sealed class TraitService : Repository<Trait>, ITraitService
    {
        public TraitService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}