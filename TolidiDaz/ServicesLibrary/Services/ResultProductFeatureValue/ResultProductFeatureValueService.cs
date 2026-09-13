using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductFeatureValueSrv
{
    public sealed class ProductFeatureValueService : Repository<ProductFeatureValue>, IProductFeatureValueService
    {
        public ProductFeatureValueService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}