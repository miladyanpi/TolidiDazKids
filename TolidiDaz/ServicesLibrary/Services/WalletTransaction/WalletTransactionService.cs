using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.WalletTransactionSrv
{
    public sealed class WalletTransactionService : Repository<WalletTransaction>, IWalletTransactionService
    {

        public WalletTransactionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}