using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.PersonelSrv
{
    public sealed class PersonelService:Repository<Personel>, IPersonelService
    {
        public PersonelService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
