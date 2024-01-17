using Savi_Thrift.Application.Repositories;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IWalletRepository WalletRepository { get; }
		IWalletFundingRepository WalletFundingRepository { get; }
		ISavingRepository SavingRepository { get; }
		IUserRepository UserRepository { get; }
		IGroupRepository GroupRepository { get; }
        IKycRepository KycRepository { get; }
        Task<int> SaveChangesAsync();
	}
}
