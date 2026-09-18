using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductVariantValueSrv
{
    public sealed class ProductVariantValueService : Repository<ProductVariantValue>, IProductVariantValueService
    {
        public ProductVariantValueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}