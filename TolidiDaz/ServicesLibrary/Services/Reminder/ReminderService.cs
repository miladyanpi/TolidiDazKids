using DAL.Context;
using Domain;
using Dto.Services.ReminderSrv;

namespace ServicesLibrary.Services.ReminderSrv
{
    public sealed class ReminderService : Repository<Reminder>, IReminderService
    {

        public ReminderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}