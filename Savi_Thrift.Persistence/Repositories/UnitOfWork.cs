

using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Application.Repositories;
using Savi_Thrift.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			GroupRepository = new GroupRepository(_saviDbContext);
			WalletFundingRepository = new WalletFundingRepository(_saviDbContext);
            KycRepository = new KycRepository(_saviDbContext);
        
            UserTransactionRepository = new UserTransactionRepository(_saviDbContext);

        }

		public IWalletRepository WalletRepository { get; set; }
		public ISavingRepository SavingRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
		public IGroupRepository GroupRepository { get; set; }
		public IWalletFundingRepository WalletFundingRepository { get; set; }
        public IKycRepository KycRepository { get; private set; }
		public IUserTransactionRepository UserTransactionRepository { get; set; }



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
