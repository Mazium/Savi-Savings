using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class DefaultingUserRepository: GenericRepository<DefaultingUsers>, IDefaultingUserRepository
    {
        public DefaultingUserRepository(SaviDbContext saviDbContext) : base(saviDbContext) { }
    }
}
