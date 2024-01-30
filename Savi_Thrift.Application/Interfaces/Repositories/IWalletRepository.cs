using Savi_Thrift.Domain.Entities;
using System.Linq.Expressions;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
	public interface IWalletRepository : IGenericRepository<Wallet>
	{
        Task<List<Wallet>> GetWalletsAsync();
        Task AddWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(Wallet wallet);
        Task DeleteAllWalletAsync(List<Wallet> wallets);
        void UpdateWalletAsync(Wallet wallet);
        Task<List<Wallet>> FindWallets(Expression<Func<Wallet, bool>> expression);
        Task<Wallet> GetWalletByIdAsync(string id);
        Wallet WalletById(string userId);
    }
}
