using DAL.Context;
using Domain;

namespace  Dto.Services.SmsLogSrv
{
    public sealed class SmsLogService : Repository<SmsLog>, ISmsLogService
    {
        public SmsLogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}