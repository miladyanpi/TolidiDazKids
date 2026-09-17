using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductVariantSrv
{
    public sealed class ProductVariantService : Repository<ProductVariant>, IProductVariantService
    {
        public ProductVariantService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}