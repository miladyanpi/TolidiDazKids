using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.RawProductStore_ProductSrv
{
    public sealed class RawProductStore_ProductService : Repository<RawProductStore_Product>, IRawProductStore_ProductService
    {
        public RawProductStore_ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}