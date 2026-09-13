using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.WalletSrv
{
    public sealed class WalletService : Repository<Wallet>, IWalletService
    {

        public WalletService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}