using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class GroupSavingsRepository: GenericRepository<GroupSavings>, IGroupSavingsRepository
    {
        private readonly SaviDbContext _saviDbContext;

        public GroupSavingsRepository(SaviDbContext saviDbContext) : base(saviDbContext)
        {
            _saviDbContext = saviDbContext;
        }
    }
}
