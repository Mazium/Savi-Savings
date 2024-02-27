using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Repositories;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
	{
		private readonly SaviDbContext _saviDbContext;

		public UnitOfWork(SaviDbContext saviDbContext)
		{
			_saviDbContext = saviDbContext;
			 WalletRepository = new WalletRepository(_saviDbContext);
			SavingRepository = new SavingRepository(_saviDbContext);
			UserRepository = new UserRepository(_saviDbContext);
			GroupSavingsRepository = new GroupSavingsRepository(_saviDbContext);
			WalletFundingRepository = new WalletFundingRepository(_saviDbContext);
            KycRepository = new KycRepository(_saviDbContext);
            GroupMembersRepository = new GroupMembersRepository(_saviDbContext);
			GroupTransactionRepository = new GroupTransactionRepository(_saviDbContext);
            UserTransactionRepository = new UserTransactionRepository(_saviDbContext);
			DefaultingUserRepository = new DefaultingUserRepository(_saviDbContext);

        }

		public IGroupTransactionRepository GroupTransactionRepository { get; set; }
		public IWalletRepository WalletRepository { get; set; }
		public ISavingRepository SavingRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
        public IGroupSavingsRepository GroupSavingsRepository { get; set; }
        public IGroupMembersRepository GroupMembersRepository { get; set; }

        public IWalletFundingRepository WalletFundingRepository { get; set; }
        public IKycRepository KycRepository { get; private set; }
		public IUserTransactionRepository UserTransactionRepository { get; set; }
<<<<<<< HEAD
        public IDefaultingUserRepository DefaultingUserRepository { get; set; }
=======
		public IDefaultingUserRepository DefaultingUserRepository { get; set; }
>>>>>>> develop



        public async Task<int> SaveChangesAsync()
		{
			return await _saviDbContext.SaveChangesAsync();
		}
		public void Dispose()
		{
			_saviDbContext.Dispose();
		}
	}
}
