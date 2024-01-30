using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;
using System.Linq.Expressions;

namespace Savi_Thrift.Persistence.Repositories
{
	public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
	{
		private readonly SaviDbContext _context;
		public WalletRepository(SaviDbContext context) : base(context)
		{
			_context = context;
		}

        public async Task AddWalletAsync(Wallet wallet)
        {
            await AddAsync(wallet);
        }

        public async Task DeleteAllWalletAsync(List<Wallet> wallets)
        {
            DeleteAllAsync(wallets);
        }

        public async Task DeleteWalletAsync(Wallet wallet)
        {
            DeleteAsync(wallet);
        }

        public async Task<List<Wallet>> FindWallets(Expression<Func<Wallet, bool>> expression)
        {
            return await FindAsync(expression);
        }
        public async Task<Wallet> GetWalletByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<List<Wallet>> GetWalletsAsync()
        {
            return await GetAllAsync();
        }

        public void UpdateWalletAsync(Wallet wallet)
        {
            Update(wallet);
        }
        public Wallet WalletById(string userId)
        {
            return _context.Wallets.FirstOrDefault(w => w.UserId == userId);
        }
    }
}
