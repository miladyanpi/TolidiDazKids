using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ProductSrv
{
    public interface IProductService:IRepository<Product>
    {
    }
}
