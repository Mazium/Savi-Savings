using Savi_Thrift.Application.Repositories;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IWalletRepository WalletRepository { get; }
		IWalletFundingRepository WalletFundingRepository { get; }
        IUserTransactionRepository UserTransactionRepository { get; }
        ISavingRepository SavingRepository { get; }
        IGroupMembersRepository GroupMembersRepository { get; }
        IUserRepository UserRepository { get; }
		IGroupSavingsRepository GroupSavingsRepository { get; }
        IKycRepository KycRepository { get; }
		IGroupTransactionRepository GroupTransactionRepository { get; }
		IDefaultingUserRepository DefaultingUserRepository { get; }
        Task<int> SaveChangesAsync();
	}
}
