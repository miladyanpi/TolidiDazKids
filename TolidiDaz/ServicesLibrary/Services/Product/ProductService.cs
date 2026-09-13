using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductSrv
{
    public sealed class ProductService:Repository<Product>, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
