using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.TicketSrv
{
    public sealed class TicketService : Repository<Ticket>, ITicketService
    {
        public TicketService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}