using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.FaqSrv
{
    public sealed class FaqService : Repository<Faq>, IFaqService
    {
        public FaqService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}