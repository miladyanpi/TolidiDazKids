using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.TeamSrv
{
    public sealed class TeamService : Repository<Team>, ITeamService
    {
        public TeamService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}