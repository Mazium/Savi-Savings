

using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
	public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
	{
		public WalletRepository(SaviDbContext context) : base(context) { }
	

	}
}
