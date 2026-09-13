using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductCommentSrv
{
    public sealed class ProductCommentService : Repository<ProductComment>, IProductCommentService
    {
        public ProductCommentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}