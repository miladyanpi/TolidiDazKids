using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductFeatureSrv
{
    public sealed class ProductFeatureService : Repository<ProductFeature>, IProductFeatureService
    {
        public ProductFeatureService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}