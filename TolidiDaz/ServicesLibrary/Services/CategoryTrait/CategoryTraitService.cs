using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CategoryTraitSrv
{
    public sealed class CategoryTraitService : Repository<CategoryTrait>, ICategoryTraitService
    {
        public CategoryTraitService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}