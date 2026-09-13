using DAL.Context;
using Domain;

namespace  Dto.Services.ReminderEventSrv
{
    public sealed class ReminderEventService : Repository<ReminderEvent>, IReminderEventService
    {

        public ReminderEventService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}