using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.BlogSrv
{
    public sealed class BlogService : Repository<Blog>, IBlogService
    {
        public BlogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}