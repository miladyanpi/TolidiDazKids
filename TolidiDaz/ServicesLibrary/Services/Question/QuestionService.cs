using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.QuestionSrv
{
    public sealed class QuestionService : Repository<Question>, IQuestionService
    {
        public QuestionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}