using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CategorySrv
{
    public sealed class CategoryService:Repository<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
