using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.SendProductMethodSrv
{
    public sealed class SendProductMethodService : Repository<SendProductMethod>, ISendProductMethodService
    {
        public SendProductMethodService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}