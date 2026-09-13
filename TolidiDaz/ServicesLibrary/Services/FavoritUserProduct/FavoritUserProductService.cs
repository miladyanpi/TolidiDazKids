using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.FavoritUserProductSrv
{
    public sealed class FavoritUserProductService:Repository<FavoritUserProduct>, IFavoritUserProductService
    {
        public FavoritUserProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
