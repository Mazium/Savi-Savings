using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class GroupTransactionRepository : GenericRepository<GroupTransactions>, IGroupTransactionRepository
    {
        private readonly SaviDbContext _saviDbContext;

        public GroupTransactionRepository(SaviDbContext saviDbContext) : base(saviDbContext)
        {
            _saviDbContext = saviDbContext;
        }
        

    }
}
