using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.StorySrv
{
    public sealed class StoryService : Repository<Story>, IStoryService
    {
        public StoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}