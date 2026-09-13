using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.CategorySrv
{
    public interface ICategoryService:IRepository<Category>
    {
    }
}
