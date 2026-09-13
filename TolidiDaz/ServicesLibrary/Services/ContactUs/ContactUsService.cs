using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.ContactUsSrv
{
    public sealed class ContactUsService : Repository<ContactUs>, IContactUsService
    {
        public ContactUsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}