using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class GroupTransactionRepository : GenericRepository<UserTransaction>, IGroupTransactionRepository
    {
        private readonly SaviDbContext _saviDbContext;

        public GroupTransactionRepository(SaviDbContext saviDbContext) : base(saviDbContext)
        {
            _saviDbContext = saviDbContext;
        }
        public async Task<List<GroupTransaction>> GetNewGroupTransactions()
        {
            DateTime today = DateTime.Today;
            var newUsers = await _saviDbContext.GroupTransactions.Where(u => u.CreatedAt.Date == today)
                               .ToListAsync();
            return newUsers;
        }

    }
}
