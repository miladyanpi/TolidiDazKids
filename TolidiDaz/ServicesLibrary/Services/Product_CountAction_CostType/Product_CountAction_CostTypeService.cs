using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.Product_CountAction_CostTypeSrv
{
    public sealed class Product_CountAction_CostTypeService : Repository<Product_CountAction_CostType>, IProduct_CountAction_CostTypeService
    {
        public Product_CountAction_CostTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}