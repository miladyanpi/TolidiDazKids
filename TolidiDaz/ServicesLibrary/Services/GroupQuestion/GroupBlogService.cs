using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.GroupQuestionSrv
{
    public sealed class GroupQuestionService : Repository<GroupQuestion>, IGroupQuestionService
    {
        public GroupQuestionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}