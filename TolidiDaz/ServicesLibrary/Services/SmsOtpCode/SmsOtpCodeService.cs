using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.SmsOtpCodeSrv
{
    public sealed class SmsOtpCodeService : Repository<SmsOtpCode>, ISmsOtpCodeService
    {
        public SmsOtpCodeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}