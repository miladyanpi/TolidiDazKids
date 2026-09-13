using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.BlogCommentSrv
{
    public sealed class BlogCommentService : Repository<BlogComment>, IBlogCommentService
    {
        public BlogCommentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}